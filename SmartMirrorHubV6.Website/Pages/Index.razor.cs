namespace SmartMirrorHubV6.Website.Pages;

public partial class Index : BaseComponent
{
    protected override void OnInitialized()
    {
        base.OnInitialized();
#if DEBUG
        NavigationManager.NavigateTo("/mirror/1/Dev");
#else
        NavigationManager.NavigateTo($"/mirror/{Configuration["UserId"]}/{Configuration["MirrorName"]}");
#endif
    }
}
