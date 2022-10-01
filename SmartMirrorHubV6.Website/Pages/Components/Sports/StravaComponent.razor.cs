using SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;

namespace SmartMirrorHubV6.Website.Pages.Components.Sports;

public partial class StravaComponent : MirrorGenericBaseComponent<StravaResponse>
{
    public override string ComponentAuthor => "Strava";
    public override string ComponentName => "Strava Stats";
}
