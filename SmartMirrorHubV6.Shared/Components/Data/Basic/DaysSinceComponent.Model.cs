using SmartMirrorHubV6.Shared.Components.Base;

namespace SmartMirrorHubV6.Shared.Components.Data.Basic;

public class DaysSinceResponse : ComponentResponse
{
    public int NumberOfDays { get; set; }
    public string EventName { get; set; }
    public bool ShowName { get; set; }
    public string Icon { get; set; }
}
