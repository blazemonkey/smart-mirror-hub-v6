using SmartMirrorHubV6.Api.Models;

namespace SmartMirrorHubV6.Api.Hubs;

public interface IMirrorHub
{
    Task RefreshMirrorComponents(int userId, string mirrorName, RefreshComponentResponse[] componentResponse);
    Task ToggleMirrorComponents(int userId, string mirrorName, bool show, string[] mirrorComponentName);
    Task SwitchMirrorLayer(int userId, string mirrorName, int layer);
}
