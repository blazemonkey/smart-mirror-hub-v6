using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Weather;

public class OpenWeatherForecastMain
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
    [JsonPropertyName("sea_level")]
    public double SeaLevel { get; set; }
    [JsonPropertyName("grnd_level")]
    public double GroundLevel { get; set; }
}

public class OpenWeatherForecastWeather
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

public class OpenWeatherForecastClouds
{
    [JsonPropertyName("all")]
    public int CloudinessPercentage { get; set; }
}

public class OpenWeatherForecastWind
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }
    [JsonPropertyName("deg")]
    public double DirectionDegrees { get; set; }
}

public class OpenWeatherForecastRain
{
    [JsonPropertyName("3h")]
    public double ThreeHourVolume { get; set; }
}

public class OpenWeatherForecastList
{
    [JsonPropertyName("dt")]
    public int TimestampUnix { get; set; }
    [JsonPropertyName("main")]
    public OpenWeatherForecastMain Main { get; set; }
    [JsonPropertyName("weather")]
    public List<OpenWeatherForecastWeather> Weather { get; set; }
    [JsonPropertyName("clouds")]
    public OpenWeatherForecastClouds Clouds { get; set; }
    [JsonPropertyName("wind")]
    public OpenWeatherForecastWind Wind { get; set; }
    [JsonPropertyName("rain")]
    public OpenWeatherForecastRain Rain { get; set; }
}

public class OpenWeatherForecastCoord
{
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
}

public class OpenWeatherForecastCity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("coord")]
    public OpenWeatherForecastCoord Coordinates { get; set; }
    [JsonPropertyName("country")]
    public string CountryCode { get; set; }
    [JsonPropertyName("population")]
    public int Population { get; set; }
}

public class OpenWeatherForecastRoot
{
    [JsonPropertyName("cnt")]
    public int Count { get; set; }
    [JsonPropertyName("list")]
    public List<OpenWeatherForecastList> Forecasts { get; set; }
    [JsonPropertyName("city")]
    public OpenWeatherForecastCity City { get; set; }
}

public class OpenWeatherForecastResponse : ComponentResponse
{
    public string Name { get; set; }
    public List<OpenWeatherForecastDetailResponse> Forecasts { get; set; }

    public OpenWeatherForecastResponse()
    {
        Forecasts = new List<OpenWeatherForecastDetailResponse>();
    }

    public static explicit operator OpenWeatherForecastResponse(OpenWeatherForecastRoot forecast)
    {
        var response = new OpenWeatherForecastResponse
        {
            Name = forecast.City.Name
        };

        foreach (var f in forecast.Forecasts)
        {
            var detail = new OpenWeatherForecastDetailResponse
            {
                Group = f.Weather?.FirstOrDefault()?.Group,
                Icon = f.Weather?.FirstOrDefault()?.Icon,
                Description = f.Weather?.FirstOrDefault()?.Description,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(f.TimestampUnix).UtcDateTime,
                Temperature = f?.Main.Temperature,
                Humidity = f?.Main.Humidity,
                Rain = f?.Rain?.ThreeHourVolume
            };

            response.Forecasts.Add(detail);
        }

        return response;
    }
}

public class OpenWeatherForecastDetailResponse
{
    public DateTime Timestamp { get; set; }
    public string Group { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? Rain { get; set; }
}
