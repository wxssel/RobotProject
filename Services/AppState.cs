public class AppState
{
    public string BatteryPercentage { get; set; } = "0%";

    // Dit event laat weten wanneer de batterij verandert
    public event Action? OnChange;

    public void NotifyStateChanged() => OnChange?.Invoke();
}
