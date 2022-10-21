namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorComponent : BaseModel
{
    public int MirrorId { get; set; }
    public int ComponentId { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public string Schedule { get; set; }    
    public MirrorComponentSetting[] Settings { get; set; }
    public MirrorComponentUiElement UiElement { get; set; } = new MirrorComponentUiElement();
    public int? TokenId { get; set; }
    public DateTime LastUpdatedTimeUtc { get; set; }
    public bool InSchedule { get; set; }

    public bool ShowMirrorComponent(MirrorComponent mirrorComponent, Mirror mirror)
    {
#if DEBUG
        return true;
#else
        var mirrorSchedule = mirror.Schedule;
        var mirrorComponentSchedule = mirrorComponent.Schedule;

        if (mirrorSchedule.Length != mirrorComponentSchedule.Length)
            return false;

        try
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(mirror.Timezone);
            var time = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);

            var component = mirrorComponentSchedule.Substring((int)time.DayOfWeek * 96, 96); // day
            component = component.Substring(time.Hour * 4, 4); // hour

            var on = component.Substring(time.Minute / 15, 1); // 15-minute block
            if (on == "0")
                return false;

            var global = mirror.Schedule.Substring((int)time.DayOfWeek * 96, 96); // day
            global = global.Substring(time.Hour * 4, 4); // hour

            var globalOn = component.Substring(time.Minute / 15, 1); // 15-minute block
            return globalOn == "1";
        }
        catch (Exception ex)
        {
            return false;
        }
#endif
    }
}
