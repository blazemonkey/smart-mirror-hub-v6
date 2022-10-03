using Microsoft.AspNetCore.SignalR;

namespace SmartMirrorHubV6.Api.Hubs;

public class MirrorHub : Hub<IMirrorHub>
{
    public async Task SubscribeToMirror(int userId, string mirrorName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{userId}:{mirrorName}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}:{mirrorName}");
    }
}
