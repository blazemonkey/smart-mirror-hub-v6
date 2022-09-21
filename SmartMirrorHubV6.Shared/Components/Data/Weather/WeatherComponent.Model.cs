using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Weather;

public class OpenWeatherCurrentCoord
{
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
}

public class OpenWeatherCurrentForecast
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("main")]
    public string Group { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("icon")]
    public string Icon { get; set; }
}

public class OpenWeatherCurrentMain
{
    [JsonPropertyName("temp")]
    public double Temperature { get; set; }
    [JsonPropertyName("pressure")]
    public double Pressure { get; set; }
    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }
    [JsonPropertyName("temp_min")]
    public double TemperatureMin { get; set; }
    [JsonPropertyName("temp_max")]
    public double TemperatureMax { get; set; }
}

public class OpenWeatherCurrentWind
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }
    [JsonPropertyName("deg")]
    public double DirectionDegrees { get; set; }
}

public class OpenWeatherCurrentClouds
{
    [JsonPropertyName("all")]
    public int CloudinessPercentage { get; set; }
}

public class OpenWeatherCurrentSys
{
    [JsonPropertyName("type")]
    public int Type { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("message")]
    public double Message { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("sunrise")]
    public long Sunrise { get; set; }
    [JsonPropertyName("sunset")]
    public long Sunset { get; set; }
}

public class OpenWeatherCurrentRoot
{
    [JsonPropertyName("coord")]
    public OpenWeatherCurrentCoord Coordinates { get; set; }
    [JsonPropertyName("weather")]
    public List<OpenWeatherCurrentForecast> Weather { get; set; }
    [JsonPropertyName("main")]
    public OpenWeatherCurrentMain Main { get; set; }
    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }
    [JsonPropertyName("wind")]
    public OpenWeatherCurrentWind Wind { get; set; }
    [JsonPropertyName("clouds")]
    public OpenWeatherCurrentClouds Clouds { get; set; }
    [JsonPropertyName("dt")]
    public int TimestampUnix { get; set; }
    [JsonPropertyName("sys")]
    public OpenWeatherCurrentSys System { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class OpenWeatherCurrentResponse : ComponentResponse
{
    public string Name { get; set; }
    public string Group { get; set; }
    public string Description { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public int? Clouds { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime SunriseTime { get; set; }
    public DateTime SunsetTime { get; set; }
    public string Icon { get; set; }

    public static explicit operator OpenWeatherCurrentResponse(OpenWeatherCurrentRoot weather)
    {
        var response = new OpenWeatherCurrentResponse()
        {
            Name = weather?.Name,
            Group = weather.Weather?.FirstOrDefault()?.Group,
            Description = weather.Weather?.FirstOrDefault()?.Description,
            Temperature = weather.Main.Temperature,
            Humidity = weather.Main.Humidity,
            Clouds = weather.Clouds?.CloudinessPercentage,
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(weather.TimestampUnix).UtcDateTime,
            SunriseTime = DateTimeOffset.FromUnixTimeSeconds(weather.System.Sunrise).UtcDateTime,
            SunsetTime = DateTimeOffset.FromUnixTimeSeconds(weather.System.Sunset).UtcDateTime,
            Icon = weather.Weather?.FirstOrDefault()?.Icon
        };

        return response;
    }
}
