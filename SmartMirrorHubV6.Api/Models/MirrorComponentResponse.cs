using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Models;

public class MirrorComponentResponse
{
    public int MirrorId { get; set; }
    public int MirrorComponentId { get; set; }
    public string Name { get; set; }
    public string ComponentName { get; set; }
    public string ComponentAuthor { get; set; }
    public bool ComponentHasJavaScript { get; set; }
    public MirrorComponentUiElement UiElement { get; set; }
    public bool InSchedule { get; set; }
}
