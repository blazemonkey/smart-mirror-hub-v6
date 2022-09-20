namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorComponentSetting : BaseModel
{
    public int MirrorComponentId { get; set; }
    public int ComponentSettingId { get; set; }
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public DateTime DateTimeValue { get; set; }
    public string JsonValue { get; set; }
}
