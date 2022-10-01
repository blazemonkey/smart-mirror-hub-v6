using SmartMirrorHubV6.Shared.Components.Data.Calendar;

namespace SmartMirrorHubV6.Website.Pages.Components.Calendar;

public partial class GoogleCalendarComponent : MirrorGenericBaseComponent<GoogleCalendarApiResponse>
{
    public override string ComponentAuthor => "Google";
    public override string ComponentName => "Google Calendar";
}
