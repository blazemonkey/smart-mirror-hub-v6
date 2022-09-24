using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Maps;

public class GoogleMapsComponent : ApiComponent
{
    public override string Name => "Map Directions";
    public override string Description => "Get directions duration time from Google Maps";
    public override string Author => "Google";
    public override string BaseUrl => "https://maps.googleapis.com/maps/api/directions/";
    public override ComponentCategory Category => ComponentCategory.Maps;
    public override ComponentType Type => ComponentType.Api;
    public override int Interval => 60 * 2;
    public override string VoiceName => "Traffic";

    #region Inputs
    [ComponentInput("Origin")]
    public string Origin { get; set; }
    [ComponentInput("Destination")]
    public string Destination { get; set; }
    [ComponentInput("Waypoints JSON")]
    public GoogleMapsWaypoint[] Waypoints { get; set; }
    [ComponentInput("Show Map")]
    public bool ShowMap { get; set; }
    [ComponentInput("Map Key")]
    public string MapKey { get; set; }
    [ComponentInput("Map Style")]
    public string MapStyle { get; set; }
    [ComponentInput("Map Width")]
    public int MapWidth { get; set; }
    [ComponentInput("Map Height")]
    public int MapHeight { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var routes = new List<GoogleMapsRouteResponse>();
        var origin = Origin.Replace(" ", "+");
        var destination = Destination.Replace(" ", "+");

        foreach (var w in Waypoints)
        {
            var route = await GetTimeInTraffic(origin, destination, w);
            if (route == null)
                continue;

            Thread.Sleep(1000);
            routes.Add(route);
        }

        var suggested = await GetTimeInTraffic(origin, destination);
        if (suggested != null)
            routes.Add(suggested);

        if (!routes.Any())
            return new ComponentResponse() { Error = "Could not get data for any routes" };

        var response = new GoogleMapsResponse()
        {
            Origin = Origin,
            Destination = Destination,
            Routes = routes.ToArray(),
            ShowMap = ShowMap,
            MapWidth = MapWidth,
            MapHeight = MapHeight
        };

        return response;
    }

    private async Task<GoogleMapsRouteResponse> GetTimeInTraffic(string origin, string destination, GoogleMapsWaypoint waypoint = null)
    {
        var query = $"{BaseUrl}json?origin={origin}&destination={destination}&departure_time=now&key={AccessToken}";
        if (waypoint != null)
        {
            var waypoints = string.Join("|", waypoint.Waypoints.Select(x => "via:" + x));
            query += $"&waypoints={waypoints}";
        }

        var result = await RestService.Instance.Get<GoogleMapsRoot>(query);
        if (result == null || result.Status != "OK")
            return null;

        var duration = result?.Routes?.FirstOrDefault()?.Legs?.FirstOrDefault()?.DurationInTraffic?.Value;
        if (!duration.HasValue)
            return null;

        var polyline = result?.Routes?.FirstOrDefault().OverviewPolyline?.Points ?? "";

        var route = new GoogleMapsRouteResponse
        {
            RouteName = waypoint?.RouteName ?? "via Suggested",
            DurationInSeconds = duration.Value,
            Duration = new TimeSpan(0, 0, duration.Value).ToString("c"),
            Polyline = polyline
        };

        return route;
    }

    public override string GetJavaScript(string uniqueName)
    {
        return "";
        //return GoogleMap.GetInit(uniqueName, MapKey, MapStyle);
    }
}
