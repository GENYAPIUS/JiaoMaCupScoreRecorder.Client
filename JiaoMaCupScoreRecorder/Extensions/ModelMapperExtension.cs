using Google.Apis.Sheets.v4.Data;
using JiaoMaCupScoreRecorder.Models;

namespace JiaoMaCupScoreRecorder.Extensions;

public static class ModelMapperExtension
{
    public static BatchUpdateValuesRequest ToBatchUpdateValuesRequestModel(
        this SheetValuesRequestDataModel sheetRequestData)
    {
        var result = new BatchUpdateValuesRequest
        {
            Data = new List<ValueRange>
            {
                new()
                {
                    MajorDimension = sheetRequestData.MajorDimension,
                    Range = sheetRequestData.Range,
                    Values = sheetRequestData.Values
                }
            },
            ValueInputOption = sheetRequestData.ValueInputOption
        };

        return result;
    }

    public static BatchUpdateSpreadsheetRequest ToNewSheetsRequestBodyModel(this SheetRequestDataModel sheetRequestData)
    {
        var result = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new()
                {
                    AddSheet = new AddSheetRequest
                    {
                        Properties = new SheetProperties
                        {
                            GridProperties = new GridProperties
                            {
                                FrozenRowCount = sheetRequestData.FrozenRowCount
                            },
                            SheetId = sheetRequestData.SheetId,
                            Title = sheetRequestData.Title,
                            Index = sheetRequestData.Index
                        }
                    }
                },
                new()
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Fields = "*",
                        Range = new GridRange
                        {
                            SheetId = sheetRequestData.SheetId,
                            StartColumnIndex = sheetRequestData.StartColumnIndex,
                            EndColumnIndex = sheetRequestData.EndColumnIndex
                        },
                        Rows = sheetRequestData.Rows
                    }
                }
            }
        };

        return result;
    }

    public static BatchUpdateSpreadsheetRequest ToUpdateSheetsRequestBodyModel(
        this SheetRequestDataModel sheetRequestData)
    {
        var result = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new()
                {
                    UpdateSheetProperties = new UpdateSheetPropertiesRequest
                    {
                        Fields = "*",
                        Properties = new SheetProperties
                        {
                            GridProperties = new GridProperties
                            {
                                FrozenRowCount = sheetRequestData.FrozenRowCount,
                                RowCount = 1000,
                                ColumnCount = 26
                            },
                            SheetId = sheetRequestData.SheetId,
                            Title = sheetRequestData.Title,
                            Index = sheetRequestData.Index
                        }
                    }
                },
                new()
                {
                    UpdateCells = new UpdateCellsRequest
                    {
                        Fields = "*",
                        Range = new GridRange
                        {
                            SheetId = sheetRequestData.SheetId,
                            StartRowIndex = sheetRequestData.StartRowIndex,
                            EndRowIndex = sheetRequestData.EndRowIndex,
                            StartColumnIndex = sheetRequestData.StartColumnIndex,
                            EndColumnIndex = sheetRequestData.EndColumnIndex
                        },
                        Rows = sheetRequestData.Rows
                    }
                }
            }
        };

        return result;
    }

    public static IList<RowData> ToRows(this IEnumerable<IEnumerable<string>> values)
    {
        return values.Select(i => new RowData
            {
                Values = i.Select(value => value[0] == '='
                        ? new CellData
                        {
                            UserEnteredValue = new ExtendedValue
                            {
                                FormulaValue = value
                            }
                        }
                        : new CellData
                        {
                            UserEnteredValue = new ExtendedValue
                            {
                                StringValue = value
                            }
                        })
                    .ToList()
            })
            .ToList();
    }
}