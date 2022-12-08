using SmartMirrorHubV6.Shared.Components.Data.Network;

namespace SmartMirrorHubV6.Website.Pages.Components.Network;

public partial class TpLinkDecoComponent : MirrorGenericBaseComponent<TpLinkDecoResponse>
{
    public override string ComponentAuthor => "TP Link Deco";
    public override string ComponentName => "Home Online";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Response == null || string.IsNullOrEmpty(Response.Error) == false)
            return;
    }
}
