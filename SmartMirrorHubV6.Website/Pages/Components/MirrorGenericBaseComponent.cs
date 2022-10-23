using Newtonsoft.Json;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Website.Pages.Components;

public abstract class MirrorGenericBaseComponent<T> : MirrorBaseComponent where T : SmartMirrorHubV6.Shared.Components.Base.ComponentResponse
{
    protected T Response { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (IsShowing == false)
            return;

        await RetrieveMirrorComponent();
    }

    private async Task RetrieveMirrorComponent()
    {
        try
        {
            var client = new MirrorApiClient(ApiUrl, HttpClient);
            var response = await client.GetLatestHistoryMirrorComponentAsync(MirrorComponentId);
            if (response == null)
                return;

            var componentResponse = JsonConvert.DeserializeObject<T>(response.ToString());
            if (string.IsNullOrEmpty(componentResponse.Error) == false)
                return;

            Response = componentResponse;
            IsShowing = true;
        }
        catch (Exception)
        {
            Response = null;
        }
    }

    public override void Update(object response)
    {
        var componentResponse = JsonConvert.DeserializeObject<T>(response.ToString());
        if (string.IsNullOrEmpty(componentResponse.Error) == false)
            IsShowing = false;
        else
        {
            Response = componentResponse;
            IsShowing = true;
        }

        InSchedule = IsShowing;
        InvokeAsync(() => StateHasChanged()).GetAwaiter().GetResult();
    }

    public override void Show()
    {
        IsShowing = true;
        InvokeAsync(() => StateHasChanged()).GetAwaiter().GetResult();
    }

    public override void Hide()
    {
        IsShowing = false;
        InvokeAsync(() => StateHasChanged()).GetAwaiter().GetResult();
    }

    public override async Task Retrieve()
    {
        await RetrieveMirrorComponent();
        await InvokeAsync(() => StateHasChanged());
    }
}
