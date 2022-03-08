namespace JiaoMaCupScoreRecorder;

public class StateContainer
{
    private bool _isGoogleLoggedIn;

    public bool IsGoogleLoggedIn
    {
        get => _isGoogleLoggedIn;
        set
        {
            _isGoogleLoggedIn = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() { OnChange?.Invoke(); }
}