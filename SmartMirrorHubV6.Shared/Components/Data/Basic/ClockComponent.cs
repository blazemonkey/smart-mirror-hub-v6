using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Data.Basic;

public class ClockComponent : BaseComponent
{
    public override string Name => "Clock";
    public override string Description => "Get the current time";
    public override string Author => "Life";
    public override ComponentCategory Category => ComponentCategory.Basic;
    public override ComponentType Type => ComponentType.None;
    public override int Interval => 1;
    public override string VoiceName => "Clock";

    #region Inputs
    [ComponentInput("Timezone")]
    public string Timezone { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var timezone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
        if (timezone == null)
            return new ComponentResponse() { Error = "Could not find timezone" };

        var dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);
        var response = new ClockResponse
        {
            DateTime = dateTime
        };

        await Task.CompletedTask;
        return response;
    }
}
