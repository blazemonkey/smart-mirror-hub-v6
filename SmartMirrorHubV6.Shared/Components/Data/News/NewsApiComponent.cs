using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.News;

public class NewsApiComponent : ApiComponent
{
    public override string Name => "News Headlines";
    public override string Description => "Get top news headlines";
    public override string Author => "News API";
    public override string BaseUrl => "http://newsapi.org/v2/top-headlines";
    public override ComponentCategory Category => ComponentCategory.News;
    public override int Interval => 60 * 60 * 24;
    public override string VoiceName => "News";

    #region Inputs
    [ComponentInput("Country Code")]
    public string CountryCode { get; set; }
    [ComponentInput("Max Number of Headlines")]
    public int MaxNumberOfHeadlines { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var result = await RestService.Instance.Get<NewsApiRoot>($"{BaseUrl}?country={CountryCode}&apiKey={AccessToken}");
        var response = (NewsApiResponse)result;
        response.Headlines = response.Headlines.Take(MaxNumberOfHeadlines).ToArray();

        return response;
    }
}
