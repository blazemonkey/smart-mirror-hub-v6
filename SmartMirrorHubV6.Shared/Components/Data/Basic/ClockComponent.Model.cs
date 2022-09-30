using SmartMirrorHubV6.Shared.Components.Base;

namespace SmartMirrorHubV6.Shared.Components.Data.Basic;

public class ClockResponse : ComponentResponse
{
    public DateTime DateTime { get; set; }
    public string Timezone { get; set; }
}
