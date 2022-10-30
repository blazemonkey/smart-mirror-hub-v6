namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorComponentDepends : BaseModel
{
    public int MirrorComponentId { get; set; }
    public int DependsMirrorComponentId { get; set; }
    public int ComponentDependsId { get; set; }
}
