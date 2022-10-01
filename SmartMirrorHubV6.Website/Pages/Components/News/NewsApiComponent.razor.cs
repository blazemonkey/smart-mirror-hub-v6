using SmartMirrorHubV6.Shared.Components.Data.News;

namespace SmartMirrorHubV6.Website.Pages.Components.News;

public partial class NewsApiComponent : MirrorGenericBaseComponent<NewsApiResponse>
{
    public override string ComponentAuthor => "News API";
    public override string ComponentName => "News Headlines";

    private int active;
    private Timer timer;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Response == null || string.IsNullOrEmpty(Response.Error) == false)
            return;

        timer = new Timer(TimerCallback, null, 10 * 1000, Timeout.Infinite);
    }

    private void TimerCallback(object state)
    {
        InvokeAsync(() =>
        {
            active++;
            if (active == Response.Headlines.Length)
                active = 0;

            StateHasChanged();
        });

        timer.Change(10 * 1000, Timeout.Infinite);
    }
}
