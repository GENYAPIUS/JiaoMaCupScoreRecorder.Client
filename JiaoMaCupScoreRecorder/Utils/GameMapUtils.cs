using JiaoMaCupScoreRecorder.Models;

namespace JiaoMaCupScoreRecorder.Utils;

public static class GameMapUtils
{
    public static IList<GameMapModel> GetGameMapList(SpreadsheetModel spreadsheet)
    {
        var gameMapList = new List<GameMapModel>
        {
            new()
            {
                GameName = GamesConst.BangDream,
                SpreadsheetId = spreadsheet.BangDream.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.Chunithm,
                SpreadsheetId = spreadsheet.CHUNITHM.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.D4Dj,
                SpreadsheetId = spreadsheet.D4DJ.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.DjMax,
                SpreadsheetId = spreadsheet.DJMAX.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.Maimai,
                SpreadsheetId = spreadsheet.Maimai.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.ProjectSekai,
                SpreadsheetId = spreadsheet.ProjectSekai.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.Sdvx,
                SpreadsheetId = spreadsheet.SDVX.SpreadSheetId
            },
            new()
            {
                GameName = GamesConst.Taiko,
                SpreadsheetId = spreadsheet.Taiko.SpreadSheetId
            }
        };

        return gameMapList;
    }
}