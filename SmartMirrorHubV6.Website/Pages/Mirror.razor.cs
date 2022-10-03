using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using SmartMirrorHubV6.Website.Pages.Components;
using System.Reflection;
using System.Text;

namespace SmartMirrorHubV6.Website.Pages;

public partial class Mirror : BaseComponent
{
    [Parameter]
    public int UserId { get; set; }

    [Parameter]
    public string MirrorName { get; set; }

    [Inject]
    public IWebHostEnvironment WebHostEnvironment { get; set; }

    public bool IsLoading { get; set; }

    public string ErrorMessage { get; set; }

    private RenderFragment ComponentsRender { get; set; }
    private List<MirrorBaseComponent> OnScreenMirrorComponents { get; set; }
    private HubConnection HubConnection { get; set; }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        try
        {
            OnScreenMirrorComponents = new List<MirrorBaseComponent>();
            var client = new MirrorApiClient(ApiUrl, HttpClient);
            var mirrorComponents = await client.GetAllMirrorComponentsByUserIdAndMirrorNameAsync(UserId, MirrorName);
            if (mirrorComponents == null || mirrorComponents.Any() == false)
            {
                ErrorMessage = "You've got no components set for this mirror hooman";
            }
            else
            {
                await RenderComponent(mirrorComponents.ToArray());

                if (HubConnection != null)
                    await HubConnection.StopAsync();

                HubConnection = new HubConnectionBuilder()
                    .WithUrl(HubUrl)
                    .WithAutomaticReconnect()
                    .Build();

                HubConnection.Reconnected += HubConnection_Reconnected;
                await HubConnection.StartAsync();
                if (HubConnection.State == HubConnectionState.Connected)                
                    await HubConnection.InvokeAsync("SubscribeToMirror", UserId, MirrorName);


                HubConnection.Remove("RefreshMirrorComponents");
                HubConnection.On("RefreshMirrorComponents", (int userId, string mirrorName, RefreshComponentResponse[] responses) =>
                {
                    if (userId != UserId || mirrorName != MirrorName)
                        return;

                    foreach (var r in responses)
                    {
                        var component = OnScreenMirrorComponents.FirstOrDefault(x => x.MirrorComponentId == r.MirrorComponentId);
                        if (component == null)
                            continue;

                        component.Update(r.ComponentResponse);
                    }
                });

                HubConnection.Remove("ToggleMirrorComponents");
                HubConnection.On("ToggleMirrorComponents", (int userId, string mirrorName, bool show, string[] mirrorComponentNames) =>
                {
                    if (userId != UserId || mirrorName != MirrorName)
                        return;

                    foreach (var name in mirrorComponentNames)
                    {
                        var component = OnScreenMirrorComponents.FirstOrDefault(x => x.Name == name);
                        if (component == null)
                            continue;

                        if (show)
                            component.Show();
                        else
                            component.Hide();
                    }
                });

                //    _hubConnection.On("TriggerMirrorComponents", async (int userId, string mirrorName, string[] mirrorComponentNames) =>
                //    {
                //        if (userId != UserId || mirrorName != MirrorName)
                //            return;

                //        foreach (var name in mirrorComponentNames)
                //        {
                //            var component = OnScreenMirrorComponents.FirstOrDefault(x => x.Name == name);
                //            if (component == null)
                //                continue;

                //            await component.Retrieve();
                //        }
                //    });
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error hooman error! {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task HubConnection_Reconnected(string arg)
    {
        await HubConnection.InvokeAsync("SubscribeToMirror", UserId, MirrorName);
    }

    private MirrorBaseComponent[] GetComponents()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(MirrorBaseComponent)) && x.IsAbstract == false).ToArray();
        var components = new List<MirrorBaseComponent>();
        foreach (var t in types)
        {
            var instance = (MirrorBaseComponent)Activator.CreateInstance(t);
            components.Add(instance);
        }
        return components.ToArray();
    }

    private async Task RenderComponent(MirrorComponentResponse[] components)
    {
        await WriteJavaScriptComponents(components);
        ComponentsRender = CreateComponents(components);
    }

    private RenderFragment CreateComponents(MirrorComponentResponse[] mirrorComponents) => builder =>
    {
        var components = GetComponents();
        foreach (var mc in mirrorComponents)
        {
            var component = components.FirstOrDefault(x => x.ComponentAuthor == mc.ComponentAuthor && x.ComponentName == mc.ComponentName);
            if (component == null)
                continue;

            builder.OpenElement(1, "div");
            builder.AddAttribute(1, "style", $"top: {mc.UiElement.Top}px; left: {mc.UiElement.Left}px; width: {mc.UiElement.Width}px; height: {mc.UiElement.Height}px; position: absolute;");
            builder.OpenComponent(2, component.GetType());
            builder.AddAttribute(2, nameof(MirrorBaseComponent.Name), mc.Name);
            builder.AddAttribute(2, nameof(MirrorBaseComponent.Mirror), this);
            builder.AddAttribute(2, nameof(MirrorBaseComponent.MirrorComponentId), mc.MirrorComponentId);
            builder.AddAttribute(2, nameof(MirrorBaseComponent.ShowHeader), mc.UiElement.ShowHeader);
            builder.AddAttribute(2, nameof(MirrorBaseComponent.IsShowing), true);
            builder.AddComponentReferenceCapture(2, mc => OnScreenMirrorComponents.Add((MirrorBaseComponent)mc));
            builder.CloseComponent();
            builder.CloseElement();
        }
    };

    private async Task WriteJavaScriptComponents(MirrorComponentResponse[] components)
    {
        var mirrorScriptsPath = Path.Combine(WebHostEnvironment.WebRootPath, "scripts", "mirror-scripts.js");
        if (File.Exists(mirrorScriptsPath))
            File.Delete(mirrorScriptsPath);

        var client = new MirrorApiClient(ApiUrl, HttpClient);
        foreach (var mc in components.Where(x => x.ComponentHasJavaScript))
        {
            var js = await client.GetMirrorComponentJavaScriptAsync(mc.MirrorComponentId);
            if (js == null || js.Any() == false)
                continue;

            using var streamWriter = new StreamWriter(mirrorScriptsPath, append: true);
            streamWriter.Write(Encoding.ASCII.GetString(js) + Environment.NewLine);
        }
    }
}
