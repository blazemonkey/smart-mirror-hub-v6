namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorComponent : BaseModel
{
    public int MirrorId { get; set; }
    public int ComponentId { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public string Schedule { get; set; }    
    public MirrorComponentSetting[] Settings { get; set; }
    public MirrorComponentUiElement UiElement { get; set; }
    public int? TokenId { get; set; }
    public DateTime LastUpdatedTimeUtc { get; set; }
}
