using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Models;

public class MirrorComponentResponse
{
    public int MirrorId { get; set; }
    public string Name { get; set; }
    public Component Component { get; set; }
    public MirrorComponentUiElement UiElement { get; set; }
    public object Response { get; set; }
}
