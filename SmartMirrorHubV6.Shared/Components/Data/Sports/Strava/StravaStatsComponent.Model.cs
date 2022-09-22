using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;

public class StravaRoot
{
    [JsonPropertyName("biggest_ride_distance")]
    public double BiggestRideDistance { get; set; }
    [JsonPropertyName("biggest_climb_elevation_gain")]
    public double BiggestClimbElevationGain { get; set; }
    [JsonPropertyName("recent_ride_totals")]
    public StravaActivityTotal RecentRideTotals { get; set; }
    [JsonPropertyName("recent_swim_totals")]
    public StravaActivityTotal RecentSwimTotals { get; set; }
    [JsonPropertyName("recent_run_totals")]
    public StravaActivityTotal RecentRunTotals { get; set; }
    [JsonPropertyName("ytd_ride_totals")]
    public StravaActivityTotal YearToDateRideTotals { get; set; }
    [JsonPropertyName("ytd_swim_totals")]
    public StravaActivityTotal YearToDateSwimTotals { get; set; }
    [JsonPropertyName("ytd_run_totals")]
    public StravaActivityTotal YearToDateRunTotals { get; set; }
    [JsonPropertyName("all_ride_totals")]
    public StravaActivityTotal AllRideTotals { get; set; }
    [JsonPropertyName("all_swim_totals")]
    public StravaActivityTotal AllSwimTotals { get; set; }
    [JsonPropertyName("all_run_totals")]
    public StravaActivityTotal AllRunTotals { get; set; }
}

public class StravaActivityTotal
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("distance")]
    public double Distance { get; set; }
    [JsonPropertyName("moving_time")]
    public double MovingTime { get; set; }
    [JsonPropertyName("elapsed_time")]
    public double ElapsedTime { get; set; }
    [JsonPropertyName("elevation_gain")]
    public double ElevationGain { get; set; }
}

public class StravaResponse : ComponentResponse
{
    public StravaActivityTotalResponse RecentRunTotals { get; set; }
    public StravaActivityTotalResponse YearToDateRunTotals { get; set; }
    public StravaActivityTotalResponse AllRunTotals { get; set; }
    public StravaActivityTotalResponse RecentRideTotals { get; set; }
    public StravaActivityTotalResponse YearToDateRideTotals { get; set; }
    public StravaActivityTotalResponse AllRideTotals { get; set; }
    public StravaActivityTotalResponse RecentSwimTotals { get; set; }
    public StravaActivityTotalResponse YearToDateSwimTotals { get; set; }
    public StravaActivityTotalResponse AllSwimTotals { get; set; }

    public static explicit operator StravaResponse(StravaRoot strava)
    {
        var response = new StravaResponse();
        response.AllRunTotals = (StravaActivityTotalResponse)strava.AllRunTotals;
        response.YearToDateRunTotals = (StravaActivityTotalResponse)strava.YearToDateRunTotals;
        response.RecentRunTotals = (StravaActivityTotalResponse)strava.RecentRunTotals;
        response.AllRideTotals = (StravaActivityTotalResponse)strava.AllRideTotals;
        response.YearToDateRideTotals = (StravaActivityTotalResponse)strava.YearToDateRideTotals;
        response.RecentRideTotals = (StravaActivityTotalResponse)strava.RecentRideTotals;
        response.AllSwimTotals = (StravaActivityTotalResponse)strava.AllSwimTotals;
        response.YearToDateSwimTotals = (StravaActivityTotalResponse)strava.YearToDateSwimTotals;
        response.RecentSwimTotals = (StravaActivityTotalResponse)strava.RecentSwimTotals;
        return response;
    }
}

public class StravaActivityTotalResponse
{
    public int Count { get; set; }
    public double Distance { get; set; }
    public double ElevationGain { get; set; }
    public string MovingTime { get; set; }
    public string ElapsedTime { get; set; }

    public static explicit operator StravaActivityTotalResponse(StravaActivityTotal total)
    {
        var response = new StravaActivityTotalResponse();
        response.Count = total.Count;
        response.Distance = total.Distance;
        response.ElevationGain = total.ElevationGain;
        var elapsedTime = new TimeSpan(0, 0, (int)total.ElapsedTime);
        var movingTime = new TimeSpan(0, 0, (int)total.MovingTime);
        response.ElapsedTime = $"{ elapsedTime.Days:00}d { elapsedTime.Hours:00}h { elapsedTime.Minutes:00}m { elapsedTime.Seconds:00}s";
        response.MovingTime = $"{ movingTime.Days:00}d { movingTime.Hours:00}h { movingTime.Minutes:00}m { movingTime.Seconds:00}s";
        return response;
    }

}
