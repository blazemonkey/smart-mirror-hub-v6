namespace SmartMirrorHubV6.Api.Database.Models;

public class Token : BaseModel
{
    public string ClientId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresAt { get; set; }
    public int ExpiresIn { get; set; }
}
