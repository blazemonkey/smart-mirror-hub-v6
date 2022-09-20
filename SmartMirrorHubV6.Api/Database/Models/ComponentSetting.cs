namespace SmartMirrorHubV6.Api.Database.Models;

public class ComponentSetting : BaseModel
{
    public int ComponentId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Type { get; set; }
}
