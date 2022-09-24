using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.SocialMedia;
public class InstagramData
{
    public string Id { get; set; }
    public string Caption { get; set; }
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }
    [JsonPropertyName("media_url")]
    public string MediaUrl { get; set; }
}

public class InstagramCursors
{
    public string After { get; set; }
    public string Before { get; set; }
}

public class InstagramPaging
{
    public InstagramCursors Cursors { get; set; }
    public string Next { get; set; }
}

public class InstagramMediaListRoot
{
    public InstagramData[] Data { get; set; }
    public InstagramPaging Paging { get; set; }
}

public class InstagramMediaListResponse : ComponentResponse
{
    public string Username { get; set; }
    public int MaxImageHeight { get; set; }
    public int MaxImageWidth { get; set; }
    public int SecondsBetweenImages { get; set; }
    public string[] MediaUrls { get; set; }
}