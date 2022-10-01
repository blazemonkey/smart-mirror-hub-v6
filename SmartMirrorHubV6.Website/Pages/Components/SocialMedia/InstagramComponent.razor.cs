using SmartMirrorHubV6.Shared.Components.Data.SocialMedia;

namespace SmartMirrorHubV6.Website.Pages.Components.SocialMedia;

public partial class InstagramComponent : MirrorGenericBaseComponent<InstagramMediaListResponse>
{
    public override string ComponentAuthor => "Instagram";
    public override string ComponentName => "Instagram Media";

    private int active;
    private Timer timer;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Response == null || string.IsNullOrEmpty(Response?.Error) == false || IsShowing == false || firstRender)
        {
            if (timer != null)
            {
                await timer.DisposeAsync();
                timer = null;
            }

            return;
        }

        if (timer == null)
            timer = new Timer(TimerCallback, null, Response.SecondsBetweenImages * 1000, Timeout.Infinite);
    }

    private void TimerCallback(object state)
    {
        InvokeAsync(() =>
        {
            active++;
            if (active == Response.MediaUrls.Length)
                active = 0;

            StateHasChanged();
        });

        timer.Change(Response.SecondsBetweenImages * 1000, Timeout.Infinite);
    }
}
