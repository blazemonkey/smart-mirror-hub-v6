using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Music;

public class SpotifyRoot
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("context")]
    public Context Context { get; set; }

    [JsonPropertyName("progress_ms")]
    public long ProgressMs { get; set; }

    [JsonPropertyName("item")]
    public Item Item { get; set; }

    [JsonPropertyName("currently_playing_type")]
    public string CurrentlyPlayingType { get; set; }

    [JsonPropertyName("actions")]
    public Actions Actions { get; set; }

    [JsonPropertyName("is_playing")]
    public bool IsPlaying { get; set; }
}

public class SpotifyArtistRoot
{
    [JsonPropertyName("artists")]
    public Artist[] Artists { get; set; }
}


public class Actions
{
    [JsonPropertyName("disallows")]
    public Disallows Disallows { get; set; }
}

public class Disallows
{
    [JsonPropertyName("pausing")]
    public bool Pausing { get; set; }
}

public class Context
{
    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonPropertyName("href")]
    public Uri Href { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}

public class ExternalUrls
{
    [JsonPropertyName("spotify")]
    public Uri Spotify { get; set; }
}

public class Item
{
    [JsonPropertyName("album")]
    public Album Album { get; set; }

    [JsonPropertyName("artists")]
    public Artist[] Artists { get; set; }

    [JsonPropertyName("available_markets")]
    public string[] AvailableMarkets { get; set; }

    [JsonPropertyName("disc_number")]
    public long DiscNumber { get; set; }

    [JsonPropertyName("duration_ms")]
    public long DurationMs { get; set; }

    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }

    [JsonPropertyName("external_ids")]
    public ExternalIds ExternalIds { get; set; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonPropertyName("href")]
    public Uri Href { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("is_local")]
    public bool IsLocal { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("popularity")]
    public long Popularity { get; set; }

    [JsonPropertyName("preview_url")]
    public Uri PreviewUrl { get; set; }

    [JsonPropertyName("track_number")]
    public long TrackNumber { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}

public class Album
{
    [JsonPropertyName("album_type")]
    public string AlbumType { get; set; }

    [JsonPropertyName("artists")]
    public Artist[] Artists { get; set; }

    [JsonPropertyName("available_markets")]
    public string[] AvailableMarkets { get; set; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonPropertyName("href")]
    public Uri Href { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("images")]
    public Image[] Images { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }

    [JsonPropertyName("release_date_precision")]
    public string ReleaseDatePrecision { get; set; }

    [JsonPropertyName("total_tracks")]
    public long TotalTracks { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}

public class Artist
{
    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonPropertyName("href")]
    public Uri Href { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("images")]
    public Image[] Images { get; set; }
}

public class Image
{
    [JsonPropertyName("height")]
    public long Height { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("width")]
    public long Width { get; set; }
}

public class ExternalIds
{
    [JsonPropertyName("isrc")]
    public string Isrc { get; set; }
}

public class SpotifyArtist
{
    public string Name { get; set; }
    public string ArtistImageUrl { get; set; }
}

public class SpotifyResponse : ComponentResponse
{
    public SpotifyArtist[] Artists { get; set; }
    public string SongName { get; set; }
    public string AlbumName { get; set; }
    public string AlbumCoverUrl { get; set; }
    public long TotalTimeMs { get; set; }
    public long ProgressTimeMs { get; set; }
    public double ProgressPercentage { get; set; }

    public static explicit operator SpotifyResponse(SpotifyRoot spotify)
    {
        var response = new SpotifyResponse();
        response.Artists = spotify.Item.Artists.Select(x => new SpotifyArtist() { Name = x.Name, ArtistImageUrl = x?.Images?.FirstOrDefault()?.Url?.ToString() ?? "" }).ToArray();
        response.SongName = spotify.Item.Name;
        response.AlbumName = spotify.Item.Album.Name;
        response.AlbumCoverUrl = spotify.Item.Album.Images.FirstOrDefault().Url.ToString();
        response.TotalTimeMs = spotify.Item.DurationMs;
        response.ProgressTimeMs = spotify.ProgressMs;
        response.ProgressPercentage = (double)((double)response.ProgressTimeMs / (double)response.TotalTimeMs) * 100;
        return response;
    }
}
