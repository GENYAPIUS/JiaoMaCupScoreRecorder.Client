using Google.Apis.Sheets.v4.Data;

namespace JiaoMaCupScoreRecorder.Models;

public class SheetRequestDataModel
{
    public int SheetId { get; set; }

    public string Title { get; set; } = string.Empty;

    public IList<RowData> Rows { get; set; } = new List<RowData>();

    public int? Index { get; set; }

    public int? FrozenRowCount { get; set; }

    public int? StartColumnIndex { get; set; }

    public int? EndColumnIndex { get; set; }

    public int? StartRowIndex { get; set; }

    public int? EndRowIndex { get; set; }
}