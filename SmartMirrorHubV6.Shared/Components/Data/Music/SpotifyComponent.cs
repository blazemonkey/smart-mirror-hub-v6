using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Music;

public class SpotifyComponent : ApiOAuthComponent
{
    public override string Name => "Currently Playing Track";
    public override string Description => "Get the current playing track on Spotify";
    public override string Author => "Spotify";
    public override string BaseUrl => "https://api.spotify.com/";
    public override ComponentCategory Category => ComponentCategory.Music;
    public override int Interval => 0;
    public override string VoiceName => "Spotify";

    #region OAuth
    public override string AuthorizeUrl => "https://accounts.spotify.com/authorize";
    public override string RefreshTokenUrl => "https://accounts.spotify.com/api/token";
    public override string RefreshTokenQueryString => $"?grant_type={RefreshGrantType}&refresh_token={RefreshToken}";
    public override bool RefreshUseBearer => true;
    public override string RefreshContentType => "x-www-form-urlencoded";
    #endregion

    public override async Task<ComponentResponse> GetOAuthApi()
    {
        var result = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<SpotifyRoot>($"{BaseUrl}v1/me/player/currently-playing");
        if (result == null)
            return new ComponentResponse() { Error = "Could not get data" };

        if (result.IsPlaying == false)
            return new ComponentResponse() { Error = "No song is playing" };

        var artists = new List<SpotifyArtist>();
        var ids = string.Join(",", result.Item.Artists.Select(x => x.Id));
        
        var artistsResult = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<SpotifyArtistRoot>($"{BaseUrl}v1/artists?ids={ids}");
        var response = (SpotifyResponse)result;

        foreach (var a in response.Artists)
        {
            a.ArtistImageUrl = artistsResult.Artists.First(x => x.Name == a.Name)?.Images?.FirstOrDefault()?.Url?.ToString() ?? "";
        }

        return response;
    }
}
