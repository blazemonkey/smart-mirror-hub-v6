using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Components.Resources.JavaScript;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;

public class StravaMapComponent : ApiOAuthComponent
{
    public override string Name => "Strava Map";
    public override string Description => "Get Strava activity maps";
    public override string Author => "Strava";
    public override string BaseUrl => "https://www.strava.com/api/v3/";
    public override ComponentCategory Category => ComponentCategory.Sports;
    public override int Interval => 0;
    public override string VoiceName => "Running Map";

    #region OAuth
    public override string AuthorizeUrl => "https://www.strava.com/oauth/authorize";
    public override string RefreshTokenUrl => "https://www.strava.com/api/v3/oauth/token";
    public override string RefreshTokenQueryString => $"?client_id={ClientId}&client_secret={ClientSecret}&refresh_token={RefreshToken}&grant_type={RefreshGrantType}&redirect_uri={RedirectUri}";
    #endregion

    #region Inputs
    [ComponentInput("Map Key")]
    public string MapKey { get; set; }
    [ComponentInput("Map Style")]
    public string MapStyle { get; set; }
    [ComponentInput("Show Stats")]
    public bool ShowStats { get; set; }
    [ComponentInput("Activity IDs")]
    public string[] ActivityIds { get; set; }
    [ComponentInput("Show Latest Activity")]
    public bool ShowLatestActivity { get; set; }
    #endregion

    public override async Task<ComponentResponse> GetOAuthApi()
    {
        var stravaActivities = new List<StravaActivity>();

        if (ShowLatestActivity)
        {
            var activities = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<StravaMapRoot[]>($"{BaseUrl}athlete/activities?per_page=1");
            if (activities == null || activities.Any() == false)
                return new ComponentResponse() { Error = "No activities could be found" };

            var activity = activities.FirstOrDefault();

            var laps = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<StravaLapRoot[]>($"{BaseUrl}activities/{activity.Id}/laps");

            var stravaActivity = (StravaActivity)activity;
            stravaActivity.Laps = laps.Select(x => (StravaLapResponse)x).ToArray();
            stravaActivities.Add(stravaActivity);
        }
        else
        {
            foreach (var a in ActivityIds)
            {
                var activity = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<StravaMapRoot>($"{BaseUrl}activities/{a}");
                if (activity == null)
                    continue;

                var laps = await RestService.Instance.SetAuthorizationHeader(("Bearer", AccessToken)).Get<StravaLapRoot[]>($"{BaseUrl}activities/{a}/laps");
                var stravaActivity = (StravaActivity)activity;
                stravaActivity.Laps = laps.Select(x => (StravaLapResponse)x).ToArray();
                stravaActivities.Add(stravaActivity);
            }
        }

        var response = new StravaMapResponse()
        {
            ShowStats = ShowStats,
            Activities = stravaActivities.ToArray()
        };

        return response;
    }

    public override string GetJavaScript(string uniqueName)
    {
        return GoogleMap.GetInit(uniqueName, MapKey, MapStyle);
    }
}
