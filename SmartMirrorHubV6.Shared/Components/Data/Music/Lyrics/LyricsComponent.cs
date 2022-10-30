using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Data.Music.Lyrics;

public abstract class LyricsComponent : ApiComponent
{
    //public abstract string LyricsPageBaseUrl { get; }
    public override int Interval => 0;
    public override ComponentCategory Category => ComponentCategory.Music;
    public string SongName { get; set; }
    public string ArtistNames { get; set; }

    #region Inputs
    [ComponentInput("Max Column Show Count")]
    public int MaxColumnShowCount { get; set; }

    [ComponentInput("Max Lines Per Column")]
    public int MaxLinesPerColumn { get; set; }

    [ComponentDepends("Stream Provider Component", true)]
    public SpotifyResponse StreamProviderComponent { get; set; }
    #endregion

    public abstract Task<LyricsResponse> GetLyrics();

    protected override async sealed Task<ComponentResponse> Get()
    {
        ArtistNames = string.Join(" ", StreamProviderComponent.Artists.Select(x => x.Name));
        SongName = StreamProviderComponent.SongName;
        
        var response = new LyricsResponse();
        if (string.IsNullOrEmpty(response.Error) == false)
            return new ComponentResponse() { Error = "No lyrics could be found" };
        else
        {
            response = await GetLyrics();
            response.ArtistNames = ArtistNames;
            response.SongName = SongName;
        }

        return response;
    }
}