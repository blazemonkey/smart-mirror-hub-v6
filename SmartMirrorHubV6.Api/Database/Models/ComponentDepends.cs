namespace SmartMirrorHubV6.Api.Database.Models;

public class ComponentDepends : BaseModel
{
    public int ComponentId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
    public bool GetLatest { get; set; }
}
