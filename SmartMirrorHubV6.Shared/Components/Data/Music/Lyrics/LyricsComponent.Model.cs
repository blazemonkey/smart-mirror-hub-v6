using SmartMirrorHubV6.Shared.Components.Base;

namespace SmartMirrorHubV6.Shared.Components.Data.Music.Lyrics;

public class LyricsSearchResult
{
    public string ArtistName { get; set; }
    public string SongName { get; set; }
    public string UrlLink { get; set; }
}

public class LyricsResponse : ComponentResponse
{
    public string ArtistNames { get; set; }
    public string SongName { get; set; }
    public string[] LyricsPages { get; set; }
}
