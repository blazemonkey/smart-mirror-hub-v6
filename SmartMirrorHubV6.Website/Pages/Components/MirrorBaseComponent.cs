using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SmartMirrorHubV6.Website.Pages.Components;

public abstract class MirrorBaseComponent : BaseComponent
{
    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public Mirror Mirror { get; set; }

    [Parameter]
    public int MirrorComponentId { get; set; }

    [Parameter]
    public bool ShowHeader { get; set; }

    [Parameter]
    public bool IsShowing { get; set; }

    [Parameter]
    public bool InSchedule { get; set; }

    [Parameter]
    public int Layer { get; set; }

    [Inject]
    public IJSRuntime JsRuntime { get; set; }
    public virtual bool IsOverlay() { return false; }

    public abstract string ComponentAuthor { get; }
    public abstract string ComponentName { get; }

    public abstract Task Retrieve();
    public abstract void Update(object response);
    public abstract void Show();
    public abstract void Hide();
}
