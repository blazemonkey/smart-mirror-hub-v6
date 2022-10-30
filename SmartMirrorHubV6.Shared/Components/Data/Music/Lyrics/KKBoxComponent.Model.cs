using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Music.Lyrics;

public class Album
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Cover { get; set; }
    public Artist Artist { get; set; }
}

public class Artist
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class ArtistRole
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class Data
{
    public string Type { get; set; }
    public List<Result> Result { get; set; }
}

public class Result
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    [JsonPropertyName("short_url")]
    public string ShortUrl { get; set; }
    public Album Album { get; set; }
    [JsonPropertyName("artist_roles")]
    public List<ArtistRole> ArtistRoles { get; set; }
    public int Duration { get; set; }
    public bool Explicitness { get; set; }
    [JsonPropertyName("has_lyrics")]

    public bool HasLyrics { get; set; }
}

public class KKBoxRoot
{
    public string Status { get; set; }
    public Data Data { get; set; }
}
