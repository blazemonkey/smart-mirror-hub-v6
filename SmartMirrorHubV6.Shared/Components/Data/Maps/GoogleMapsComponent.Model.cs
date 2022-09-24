using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Maps;

public class GoogleMapsWaypoint
{
    [JsonPropertyName("routeName")]

    public string RouteName { get; set; }
    [JsonPropertyName("wayPoints")]

    public string[] Waypoints { get; set; }
}

public class GeocodedWaypoint
{
    [JsonPropertyName("geocoder_status")]
    public string GeocoderStatus { get; set; }
    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; }
    [JsonPropertyName("types")]
    public List<string> Types { get; set; }
}

public class Northeast
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public class Southwest
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public class Bounds
{
    [JsonPropertyName("northeast")]
    public Northeast Northeast { get; set; }
    [JsonPropertyName("southwest")]
    public Southwest Southwest { get; set; }
}

public class Distance
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    [JsonPropertyName("value")]
    public int Value { get; set; }
}

public class Duration
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    [JsonPropertyName("value")]
    public int Value { get; set; }
}

public class DurationInTraffic
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
    [JsonPropertyName("value")]
    public int Value { get; set; }
}

public class EndLocation
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public class StartLocation
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public class Polyline
{
    [JsonPropertyName("points")]
    public string Points { get; set; }
}

public class Step
{
    [JsonPropertyName("distance")]
    public Distance Distance { get; set; }
    [JsonPropertyName("duration")]
    public Duration Duration { get; set; }
    [JsonPropertyName("end_location")]
    public EndLocation EndLocation { get; set; }
    [JsonPropertyName("html_instructions")]
    public string HtmlInstructions { get; set; }
    [JsonPropertyName("polyline")]
    public Polyline Polyline { get; set; }
    [JsonPropertyName("start_location")]
    public StartLocation StartLocation { get; set; }
    [JsonPropertyName("travel_mode")]
    public string TravelMode { get; set; }
    [JsonPropertyName("maneuver")]
    public string Maneuver { get; set; }
}

public class Location
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lng")]
    public double Longitude { get; set; }
}

public class ViaWaypoint
{
    [JsonPropertyName("location")]
    public Location Location { get; set; }
    [JsonPropertyName("step_index")]
    public int StepIndex { get; set; }
    [JsonPropertyName("step_interpolation")]
    public double StepInterpolation { get; set; }
}

public class Leg
{
    [JsonPropertyName("distance")]
    public Distance Distance { get; set; }
    [JsonPropertyName("duration")]
    public Duration Duration { get; set; }
    [JsonPropertyName("duration_in_traffic")]
    public DurationInTraffic DurationInTraffic { get; set; }
    [JsonPropertyName("end_address")]
    public string EndAddress { get; set; }
    [JsonPropertyName("end_location")]
    public EndLocation EndLocation { get; set; }
    [JsonPropertyName("start_address")]
    public string StartAddress { get; set; }
    [JsonPropertyName("start_location")]
    public StartLocation StartLocation { get; set; }
    [JsonPropertyName("steps")]
    public List<Step> Steps { get; set; }
    [JsonPropertyName("traffic_speed_entry")]
    public List<object> TrafficSpeedEntry { get; set; }
    [JsonPropertyName("via_waypoint")]
    public List<ViaWaypoint> ViaWaypoint { get; set; }
}

public class OverviewPolyline
{
    [JsonPropertyName("points")]
    public string Points { get; set; }
}

public class Route
{
    [JsonPropertyName("bounds")]
    public Bounds Bounds { get; set; }
    [JsonPropertyName("copyrights")]
    public string Copyrights { get; set; }
    [JsonPropertyName("legs")]
    public List<Leg> Legs { get; set; }
    [JsonPropertyName("overview_polyline")]
    public OverviewPolyline OverviewPolyline { get; set; }
    [JsonPropertyName("summary")]
    public string Summary { get; set; }
    [JsonPropertyName("warnings")]
    public List<object> Warnings { get; set; }
    [JsonPropertyName("waypoint_order")]
    public List<object> WaypointOrder { get; set; }
}

public class GoogleMapsRoot
{
    [JsonPropertyName("geocoded_waypoints")]
    public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }
    [JsonPropertyName("routes")]
    public List<Route> Routes { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class GoogleMapsRouteResponse
{
    public string RouteName { get; set; }
    public int DurationInSeconds { get; set; }
    public string Duration { get; set; }
    public string Polyline { get; set; }
}

public class GoogleMapsResponse : ComponentResponse
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public GoogleMapsRouteResponse[] Routes { get; set; }
    public bool ShowMap { get; set; }
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
}
