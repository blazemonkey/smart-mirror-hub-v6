using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;

public class StravaStatsComponent : ApiOAuthComponent
{
    public override string Name => "Strava Stats";
    public override string Description => "Get Strava activity summary data";
    public override string Author => "Strava";
    public override string BaseUrl => "https://www.strava.com/api/v3/";
    public override ComponentCategory Category => ComponentCategory.Sports;
    public override int Interval => 60 * 30;
    public override string VoiceName => "Running Stats";

    #region OAuth
    public override string AuthorizeUrl => "https://www.strava.com/oauth/authorize";
    public override string RefreshTokenUrl => "https://www.strava.com/api/v3/oauth/token";
    public override string RefreshTokenQueryString => $"?client_id={ClientId}&client_secret={ClientSecret}&refresh_token={RefreshToken}&grant_type={RefreshGrantType}&redirect_uri={RedirectUri}";
    #endregion

    #region Inputs
    [ComponentInput("Athlete Id")]
    public int AthleteId { get; set; }
    #endregion

    public override async Task<ComponentResponse> GetOAuthApi()
    {
        var result = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<StravaRoot>($"{BaseUrl}athletes/{AthleteId}/stats");
        var response = (StravaResponse)result;
        return response;
    }
}
