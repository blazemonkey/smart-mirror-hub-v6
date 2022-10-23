using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;

public class StravaAthlete
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("resource_state")]
    public int ResourceState { get; set; }
}

public class StravaMap
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("summary_polyline")]
    public string SummaryPolyline { get; set; }

    [JsonPropertyName("resource_state")]
    public int ResourceState { get; set; }
}

public class StravaMapRoot
{
    [JsonPropertyName("resource_state")]
    public int ResourceState { get; set; }

    [JsonPropertyName("athlete")]
    public StravaAthlete Athlete { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("distance")]
    public double Distance { get; set; }

    [JsonPropertyName("moving_time")]
    public int MovingTime { get; set; }

    [JsonPropertyName("elapsed_time")]
    public int ElapsedTime { get; set; }

    [JsonPropertyName("total_elevation_gain")]
    public double TotalElevationGain { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("workout_type")]
    public int? WorkoutType { get; set; }

    [JsonPropertyName("id")]
    public object Id { get; set; }

    [JsonPropertyName("external_id")]
    public string ExternalId { get; set; }

    [JsonPropertyName("upload_id")]
    public long? UploadId { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("start_date_local")]
    public DateTime StartDateLocal { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("utc_offset")]
    public double UtcOffset { get; set; }

    [JsonPropertyName("start_latlng")]
    public List<double> StartLatlng { get; set; }

    [JsonPropertyName("end_latlng")]
    public List<double> EndLatlng { get; set; }

    [JsonPropertyName("location_city")]
    public string LocationCity { get; set; }

    [JsonPropertyName("location_state")]
    public string LocationState { get; set; }

    [JsonPropertyName("location_country")]
    public string LocationCountry { get; set; }

    [JsonPropertyName("start_latitude")]
    public double? StartLatitude { get; set; }

    [JsonPropertyName("start_longitude")]
    public double? StartLongitude { get; set; }

    [JsonPropertyName("achievement_count")]
    public int AchievementCount { get; set; }

    [JsonPropertyName("kudos_count")]
    public int KudosCount { get; set; }

    [JsonPropertyName("comment_count")]
    public int CommentCount { get; set; }

    [JsonPropertyName("athlete_count")]
    public int AthleteCount { get; set; }

    [JsonPropertyName("photo_count")]
    public int PhotoCount { get; set; }

    [JsonPropertyName("map")]
    public StravaMap Map { get; set; }

    [JsonPropertyName("trainer")]
    public bool Trainer { get; set; }

    [JsonPropertyName("commute")]
    public bool Commute { get; set; }

    [JsonPropertyName("manual")]
    public bool Manual { get; set; }

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("flagged")]
    public bool Flagged { get; set; }

    [JsonPropertyName("gear_id")]
    public object GearId { get; set; }

    [JsonPropertyName("from_accepted_tag")]
    public bool? FromAcceptedTag { get; set; }

    [JsonPropertyName("upload_id_str")]
    public string UploadIdStr { get; set; }

    [JsonPropertyName("average_speed")]
    public double AverageSpeed { get; set; }

    [JsonPropertyName("max_speed")]
    public double MaxSpeed { get; set; }

    [JsonPropertyName("average_cadence")]
    public double AverageCadence { get; set; }

    [JsonPropertyName("has_heartrate")]
    public bool HasHeartrate { get; set; }

    [JsonPropertyName("average_heartrate")]
    public double AverageHeartrate { get; set; }

    [JsonPropertyName("max_heartrate")]
    public double MaxHeartrate { get; set; }

    [JsonPropertyName("heartrate_opt_out")]
    public bool HeartrateOptOut { get; set; }

    [JsonPropertyName("display_hide_heartrate_option")]
    public bool DisplayHideHeartrateOption { get; set; }

    [JsonPropertyName("elev_high")]
    public double ElevHigh { get; set; }

    [JsonPropertyName("elev_low")]
    public double ElevLow { get; set; }

    [JsonPropertyName("pr_count")]
    public int PrCount { get; set; }

    [JsonPropertyName("total_photo_count")]
    public int TotalPhotoCount { get; set; }

    [JsonPropertyName("has_kudoed")]
    public bool HasKudoed { get; set; }

    [JsonPropertyName("device_watts")]
    public bool? DeviceWatts { get; set; }

    [JsonPropertyName("average_watts")]
    public double? AverageWatts { get; set; }

    [JsonPropertyName("kilojoules")]
    public double? Kilojoules { get; set; }
}


public class StravaLapRoot
{
    public object Id { get; set; }
    [JsonPropertyName("resource_state")]
    public int ResourceState { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("elapsed_time")]
    public int ElapsedTime { get; set; }
    [JsonPropertyName("moving_time")]
    public int MovingTime { get; set; }
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }
    [JsonPropertyName("start_date_local")]
    public DateTime StartDateLocal { get; set; }
    public double Distance { get; set; }
    [JsonPropertyName("start_index")]
    public int StartIndex { get; set; }
    [JsonPropertyName("end_index")]
    public int EndIndex { get; set; }
    [JsonPropertyName("total_elevation_gain")]
    public double TotalElevationGain { get; set; }
    [JsonPropertyName("average_speed")]
    public double AverageSpeed { get; set; }
    [JsonPropertyName("max_speed")]
    public double MaxSpeed { get; set; }
    [JsonPropertyName("average_cadence")]
    public double AverageCadence { get; set; }
    [JsonPropertyName("average_heartrate")]
    public double AverageHeartrate { get; set; }
    [JsonPropertyName("max_heartrate")]
    public double MaxHeartrate { get; set; }
    [JsonPropertyName("lap_index")]
    public int LapIndex { get; set; }
    public int Split { get; set; }
    [JsonPropertyName("pace_zone")]
    public int PaceZone { get; set; }
}

public class StravaActivity
{
    public string Name { get; set; }
    public double Distance { get; set; }
    public int MovingTime { get; set; }
    public int ElapsedTime { get; set; }
    public double AverageSpeed { get; set; }
    public double MaxSpeed { get; set; }
    public double AverageHeartrate { get; set; }
    public double MaxHeartrate { get; set; }
    public double ElevationHigh { get; set; }
    public double ElevationLow { get; set; }
    public string EncodedPolyline { get; set; }
    public StravaLapResponse[] Laps { get; set; }


    public static explicit operator StravaActivity(StravaMapRoot map)
    {
        var response = new StravaActivity();
        response.Name = map.Name;
        response.Distance = map.Distance;
        response.MovingTime = map.MovingTime;
        response.ElapsedTime = map.ElapsedTime;
        response.AverageSpeed = map.AverageSpeed;
        response.MaxSpeed = map.MaxSpeed;
        response.AverageHeartrate = map.AverageHeartrate;
        response.MaxHeartrate = map.MaxHeartrate;
        response.ElevationLow = map.ElevLow;
        response.ElevationHigh = map.ElevHigh;
        response.EncodedPolyline = map?.Map?.SummaryPolyline;
        return response;
    }
}
public class StravaMapResponse : ComponentResponse
{
    public StravaActivity[] Activities { get; set; }
    public bool ShowStats { get; set; }
}

public class StravaLapResponse
{
    public string Name { get; set; }
    public int ElapsedTime { get; set; }
    public int MovingTime { get; set; }
    public double Distance { get; set; }
    public double TotalElevationGain { get; set; }
    public double AverageHeartrate { get; set; }
    public double MaxHeartrate { get; set; }
    public double AverageSpeed { get; set; }
    public double MaxSpeed { get; set; }

    public static explicit operator StravaLapResponse(StravaLapRoot lap)
    {
        var response = new StravaLapResponse();
        response.Name = lap.Name;
        response.Distance = lap.Distance;
        response.TotalElevationGain = lap.TotalElevationGain;
        response.MovingTime = lap.MovingTime;
        response.ElapsedTime = lap.ElapsedTime;
        response.AverageSpeed = lap.AverageSpeed;
        response.MaxSpeed = lap.MaxSpeed;
        response.AverageHeartrate = lap.AverageHeartrate;
        response.MaxHeartrate = lap.MaxHeartrate;
        return response;
    }
}
