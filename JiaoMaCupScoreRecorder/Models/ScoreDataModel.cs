namespace JiaoMaCupScoreRecorder.Models;

public class ScoreDataModel
{
    public string DiscordId { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public int Point { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}