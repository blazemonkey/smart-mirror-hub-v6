using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Calendar;

public class GoogleCalendarComponent : ApiOAuthComponent
{
    public override string Name => "Google Calendar";
    public override string Description => "Get events from Google Calendar";
    public override string Author => "Google";
    public override string BaseUrl => "https://www.googleapis.com/calendar/v3";
    public override ComponentCategory Category => ComponentCategory.Calendar;
    public override int Interval => 60 * 60 * 12;
    public override string VoiceName => "Calendar";

    #region OAuth
    public override string AuthorizeUrl => "https://accounts.google.com/o/oauth2/v2/auth";
    public override string RefreshTokenUrl => "https://oauth2.googleapis.com/token";
    public override string RefreshTokenQueryString => $"?client_id={ClientId}&client_secret={ClientSecret}&refresh_token={RefreshToken}&grant_type={RefreshGrantType}&redirect_uri={RedirectUri}";
    #endregion

    #region Inputs
    [ComponentInput("Calendars")]
    public string[] Calendars { get; set; }
    [ComponentInput("Max")]
    public int Max { get; set; }
    #endregion

    public override async Task<ComponentResponse> GetOAuthApi()
    {
        var calendarList = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<GoogleCalendarListRoot>($"{BaseUrl}/users/me/calendarList");
        if (calendarList == null)
            return new ComponentResponse() { Error = "No calendars were found" };

        var filteredCalendars = calendarList.Items.Where(x => Calendars.Any(z => z.ToLower() == x.Summary.ToLower()));
        var calendarItems = new List<GoogleCalendarItem>();
        foreach (var c in filteredCalendars)
        {
            var calendar = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<GoogleCalendarRoot>($"{BaseUrl}/calendars/{c.Id}/events");
            calendarItems.AddRange(calendar.Items);
        }

        var response = new GoogleCalendarApiResponse
        {
            Entries = SetEventEntries(calendarItems).Where(x => x.Date.CompareTo(DateTime.UtcNow) > 0).OrderBy(x => x.Date).Take(Max).ToList()
        };

        return response;
    }

    private List<GoogleCalendarEntry> SetEventEntries(List<GoogleCalendarItem> events)
    {
        var entries = new List<GoogleCalendarEntry>();
        foreach (var e in events)
        {
            var entry = new GoogleCalendarEntry
            {
                Name = e.Summary
            };

            if (e.Start != null && e.Start.DateTime != null)
                entry.Date = e.Start.DateTime.Value;
            else if (e.Start != null && e.Start.Date != null)
                entry.Date = DateTime.ParseExact(e.Start.Date, "yyyy-MM-dd", null);

            if (e.End != null && e.End.DateTime != null)
                entry.EndDate = e.End.DateTime.Value;

            entries.Add(entry);
        }

        return entries;
    }
}
