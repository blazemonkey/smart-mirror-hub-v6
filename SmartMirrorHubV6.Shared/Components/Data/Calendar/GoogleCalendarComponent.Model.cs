using SmartMirrorHubV6.Shared.Components.Base;

namespace SmartMirrorHubV6.Shared.Components.Data.Calendar;

public class GoogleCalendarEntry
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime EndDate { get; set; }
}

public class GoogleCalendarNotification
{
    public string Type { get; set; }
    public string Method { get; set; }
}

public class GoogleCalendarNotificationSettings
{
    public List<GoogleCalendarNotification> Notifications { get; set; }
}

public class GoogleCalendarConferenceProperties
{
    public List<string> AllowedConferenceSolutionTypes { get; set; }
}

public class GoogleCalendarListItem
{
    public string Kind { get; set; }
    public string ETag { get; set; }
    public string Id { get; set; }
    public string Summary { get; set; }
    public string TimeZone { get; set; }
    public string ColorId { get; set; }
    public string BackgroundColor { get; set; }
    public string ForegroundColor { get; set; }
    public bool Selected { get; set; }
    public string AccessRole { get; set; }
    public List<object> DefaultReminders { get; set; }
    public GoogleCalendarNotificationSettings NotificationSettings { get; set; }
    public bool primary { get; set; }
    public GoogleCalendarConferenceProperties ConferenceProperties { get; set; }
    public string Description { get; set; }
    public string SummaryOverride { get; set; }
}

public class GoogleCalendarCreator
{
    public string Email { get; set; }
}

public class GoogleCalendarOrganizer
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public bool Self { get; set; }
}

public class GoogleCalendarStart
{
    public string Date { get; set; }
    public DateTime? DateTime { get; set; }
    public string TimeZone { get; set; }
}

public class GoogleCalendarEnd
{
    public string Date { get; set; }
    public DateTime? DateTime { get; set; }
    public string TimeZone { get; set; }
}

public class GoogleCalendarReminders
{
    public bool UseDefault { get; set; }
}

public class GoogleCalendarItem
{
    public string Kind { get; set; }
    public string ETag { get; set; }
    public string Id { get; set; }
    public string Status { get; set; }
    public string HtmlLink { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string Summary { get; set; }
    public GoogleCalendarCreator Creator { get; set; }
    public GoogleCalendarOrganizer Organizer { get; set; }
    public GoogleCalendarStart Start { get; set; }
    public GoogleCalendarEnd End { get; set; }
    public List<string> Recurrence { get; set; }
    public string Transparency { get; set; }
    public string ICalUID { get; set; }
    public int Sequence { get; set; }
    public GoogleCalendarReminders Reminders { get; set; }
    public string Location { get; set; }
}

public class GoogleCalendarRoot
{
    public string Kind { get; set; }
    public string ETag { get; set; }
    public string Summary { get; set; }
    public DateTime Updated { get; set; }
    public string TimeZone { get; set; }
    public string AccessRole { get; set; }
    public List<object> DefaultReminders { get; set; }
    public string NextSyncToken { get; set; }
    public List<GoogleCalendarItem> Items { get; set; }
}

public class GoogleCalendarListRoot
{
    public string Kind { get; set; }
    public string ETag { get; set; }
    public string NextSyncToken { get; set; }
    public List<GoogleCalendarListItem> Items { get; set; }
}

public class GoogleCalendarApiResponse : ComponentResponse
{
    public List<GoogleCalendarEntry> Entries { get; set; }

    public GoogleCalendarApiResponse()
    {
        Entries = new List<GoogleCalendarEntry>();
    }
}
