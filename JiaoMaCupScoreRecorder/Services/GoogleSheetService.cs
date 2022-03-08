using System.Net;
using Google.Apis.Sheets.v4.Data;
using JiaoMaCupScoreRecorder.Extensions;
using JiaoMaCupScoreRecorder.Models;
using JiaoMaCupScoreRecorder.Services.Interfaces;
using JiaoMaCupScoreRecorder.Utils;
using Microsoft.JSInterop;

namespace JiaoMaCupScoreRecorder.Services;

public class GoogleSheetService : IGoogleSheetService
{
    private readonly IList<GameMapModel> _gameMapList;
    private readonly IJSRuntime _js;

    public GoogleSheetService(IJSRuntime js, SpreadsheetModel spreadsheet)
    {
        _js = js;
        _gameMapList = GameMapUtils.GetGameMapList(spreadsheet);
    }

    public async Task<IList<IList<string>>> GetSheet(GetSheetValuesRequestBodyModel requestBody)
    {
        var requestBodyJson = JsonSerializerUtils.Serialize(requestBody);
        var result = await _js.InvokeAsync<IList<IList<string>>>("getSheetData", requestBodyJson);

        return result;
    }

    public async Task InitializeSheet(IDictionary<string, IList<IList<string>>> totalSheetDictionary,
                                      IDictionary<string, IList<IList<string>>> weekSheetDictionary)
    {
        foreach (var gameMap in _gameMapList)
        {
            await AddOrUpdateTotalSheet(gameMap, totalSheetDictionary);
            await AddOrUpdateWeekSheet(gameMap, weekSheetDictionary);
        }
    }

    public async Task BatchUpdateValues(string spreadsheetId, BatchUpdateValuesRequest requestBody)
    {
        var requestBodyJson = JsonSerializerUtils.Serialize(requestBody);

        var statusCode =
            await _js.InvokeAsync<HttpStatusCode>("batchUpdateValues", spreadsheetId, requestBodyJson);

        if (statusCode != HttpStatusCode.TooManyRequests) return;
        await Task.Delay(60000);
        _ = await _js.InvokeAsync<HttpStatusCode>("batchUpdateValues", spreadsheetId, requestBodyJson);
    }

    private async Task BatchUpdateSheet(GameMapModel gameMap, BatchUpdateSpreadsheetRequest requestBody)
    {
        var requestBodyJson = JsonSerializerUtils.Serialize(requestBody);

        var statusCode =
            await _js.InvokeAsync<HttpStatusCode>("batchUpdateSheetData", gameMap.SpreadsheetId, requestBodyJson);

        if (statusCode != HttpStatusCode.TooManyRequests) return;
        await Task.Delay(60000);
        _ = await _js.InvokeAsync<HttpStatusCode>("batchUpdateSheetData", gameMap.SpreadsheetId, requestBodyJson);
    }

    private async Task AddOrUpdateTotalSheet(GameMapModel gameMap,
                                             IDictionary<string, IList<IList<string>>> totalSheetDictionary)
    {
        var isTotalSheetExist = await IsSheetExist(gameMap, 0);

        var requestData = new SheetRequestDataModel
        {
            SheetId = 0,
            Title = "總積分",
            Index = 0,
            Rows = totalSheetDictionary[gameMap.GameName].ToRows(),
            FrozenRowCount = 1,
            StartRowIndex = 0,
            EndRowIndex = totalSheetDictionary[gameMap.GameName].Count,
            StartColumnIndex = 0,
            EndColumnIndex = 4
        };

        if (isTotalSheetExist)
        {
            var requestBody = requestData.ToUpdateSheetsRequestBodyModel();
            await BatchUpdateSheet(gameMap, requestBody);
        }
        else
        {
            var requestBody = requestData.ToNewSheetsRequestBodyModel();
            await BatchUpdateSheet(gameMap, requestBody);
        }
    }

    private async Task AddOrUpdateWeekSheet(GameMapModel gameMap,
                                            IDictionary<string, IList<IList<string>>> weekSheetDictionary)
    {
        var weekSheetExistList = await IsWeekSheetExist(gameMap);

        for (var i = 1; i < weekSheetExistList.Count; i++)
        {
            var requestData = new SheetRequestDataModel
            {
                SheetId = i,
                Title = StringConst.Sheets[i],
                Index = i,
                Rows = weekSheetDictionary[gameMap.GameName].ToRows(),
                FrozenRowCount = 1,
                StartRowIndex = 0,
                EndRowIndex = weekSheetDictionary[gameMap.GameName].Count,
                StartColumnIndex = 0,
                EndColumnIndex = 4
            };

            if (weekSheetExistList[i])
            {
                var requestBody = requestData.ToUpdateSheetsRequestBodyModel();
                await BatchUpdateSheet(gameMap, requestBody);
            }
            else
            {
                var requestBody = requestData.ToNewSheetsRequestBodyModel();
                await BatchUpdateSheet(gameMap, requestBody);
            }
        }
    }

    private async Task<IList<bool>> IsWeekSheetExist(GameMapModel gameMap)
    {
        IList<bool> result = new List<bool>();

        for (var i = 0; i < StringConst.Sheets.Length; i++)
        {
            var isExist = await IsSheetExist(gameMap, i);
            result.Add(isExist);
        }

        return result;
    }

    private async Task<bool> IsSheetExist(GameMapModel gameMap, int sheetId)
    {
        var requestBody = GetSpreadsheetByDataFilterRequestBodyModel(sheetId);
        var requestBodyJson = JsonSerializerUtils.Serialize(requestBody);

        var statusCode =
            await _js.InvokeAsync<HttpStatusCode>("checkSheetExist", gameMap.SpreadsheetId, requestBodyJson);

        if (statusCode == HttpStatusCode.TooManyRequests)
        {
            await Task.Delay(60000);

            statusCode =
                await _js.InvokeAsync<HttpStatusCode>("checkSheetExist", gameMap.SpreadsheetId, requestBodyJson);
            var result = statusCode == HttpStatusCode.OK;

            return result;
        }
        else
        {
            var result = statusCode == HttpStatusCode.OK;

            return result;
        }
    }

    private GetSpreadsheetByDataFilterRequest GetSpreadsheetByDataFilterRequestBodyModel(int sheetId) =>
        new()
        {
            DataFilters = new List<DataFilter>
            {
                new()
                {
                    GridRange = new GridRange
                    {
                        SheetId = sheetId
                    }
                }
            }
        };
}