namespace SmartMirrorHubV6.Api.Database.Models;

public class Mirror : BaseModel
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Live { get; set; }
    public string Schedule { get; set; }
    public string Timezone { get; set; }
    public MirrorComponent[] MirrorComponents { get; set; }
}
