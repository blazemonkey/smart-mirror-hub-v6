using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Data.Basic;

public class DaysSinceComponent : BaseComponent
{
    public override string Name => "Days Since";
    public override string Description => "Get the number of days since a particular date";
    public override string Author => "Life";
    public override ComponentCategory Category => ComponentCategory.Basic;
    public override ComponentType Type => ComponentType.None;
    public override int Interval => 60 * 60 * 12;
    public override string VoiceName => "Days";

    #region Inputs
    [ComponentInput("Event Name")]
    public string EventName { get; set; }

    [ComponentInput("Event Date")]
    public DateTime EventDateUtc { get; set; }

    [ComponentInput("Show Name")]
    public bool ShowName { get; set; }

    [ComponentInput("Icon")]
    public string Icon { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var totalDays = DateTime.UtcNow.Subtract(EventDateUtc).TotalDays;
        var response = new DaysSinceResponse()
        {
            NumberOfDays = (int)totalDays,
            EventName = EventName,
            ShowName = ShowName,
            Icon = Icon
        };

        await Task.CompletedTask;
        return response;
    }
}
