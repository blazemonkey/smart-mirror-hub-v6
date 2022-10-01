using SmartMirrorHubV6.Shared.Components.Data.Weather;

namespace SmartMirrorHubV6.Website.Pages.Components.Weather;

public partial class ForecastComponent : MirrorGenericBaseComponent<OpenWeatherForecastResponse>
{
    public override string ComponentAuthor => "Open Weather Map";
    public override string ComponentName => "Forecast";
}
