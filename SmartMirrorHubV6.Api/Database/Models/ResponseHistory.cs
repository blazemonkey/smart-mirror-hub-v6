namespace SmartMirrorHubV6.Api.Database.Models;

public class ResponseHistory : BaseModel
{
    public int MirrorComponentId { get; set; }
    public DateTime DateTimeUtc { get; set; }
    public bool Success { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public string Response { get; set; }
}
