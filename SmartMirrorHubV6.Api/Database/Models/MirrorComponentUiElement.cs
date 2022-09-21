namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorComponentUiElement : BaseModel
{
    public int Top { get; set; }
    public int Left { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool ShowHeader { get; set; }
    public int Layer { get; set; }
}
