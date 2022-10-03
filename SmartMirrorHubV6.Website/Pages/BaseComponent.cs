using Microsoft.AspNetCore.Components;

namespace SmartMirrorHubV6.Website.Pages;

public class BaseComponent : ComponentBase
{

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IConfiguration Configuration { get; set; }

    [Inject]
    public HttpClient HttpClient { get; set; }
    protected string ApiUrl { get; set; }
    protected string HubUrl { get; set; }

    protected override void OnInitialized()
    {
        ApiUrl = Configuration["ApiUrl"];
        HubUrl = Configuration["HubUrl"];
        base.OnInitialized();
    }
}
