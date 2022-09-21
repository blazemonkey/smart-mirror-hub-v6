using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Weather;

public class WeatherComponent : ApiComponent
{
    public override string Name => "Weather";
    public override string Description => "Get current weather data";
    public override string Author => "Open Weather Map";
    public override string BaseUrl => "https://api.openweathermap.org/data/2.5/";
    public override ComponentCategory Category => ComponentCategory.Weather;
    public override int Interval => 60 * 60;
    public override string VoiceName => "Weather";

    #region Inputs
    [ComponentInput("City Name")]
    public string CityName { get; set; }
    [ComponentInput("Country Code")]
    public string CountryCode { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var result = await RestService.Instance.Get<OpenWeatherCurrentRoot>($"{BaseUrl}weather?q={CityName},{CountryCode}&units=metric&APPID={AccessToken}");
        var response = (OpenWeatherCurrentResponse)result;
        return response;
    }
}
