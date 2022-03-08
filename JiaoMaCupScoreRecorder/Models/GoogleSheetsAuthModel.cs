namespace JiaoMaCupScoreRecorder.Models;

public class GoogleSheetsAuthModel
{
    public string ClientId { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string[] DiscoveryDocs { get; set; } = Array.Empty<string>();

    public string Scope { get; set; } = string.Empty;
}