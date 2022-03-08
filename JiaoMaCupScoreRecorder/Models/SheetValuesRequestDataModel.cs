namespace JiaoMaCupScoreRecorder.Models;

public class SheetValuesRequestDataModel
{
    public string MajorDimension { get; set; } = string.Empty;

    public string Range { get; set; } = string.Empty;

    public List<IList<object>> Values { get; set; } = new List<IList<object>>();

    public string ValueInputOption { get; set; } = string.Empty;
}