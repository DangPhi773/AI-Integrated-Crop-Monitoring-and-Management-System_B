using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs;

[Authorize]
public class IotHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetUserId();
        if (userId != Guid.Empty)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        await base.OnConnectedAsync();
    }

    public async Task JoinFarmAsync(Guid farmId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"farm:{farmId}");
    }

    public async Task LeaveFarmAsync(Guid farmId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"farm:{farmId}");
    }

    public async Task SubscribeDeviceAsync(Guid deviceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"device:{deviceId}");
    }

    public async Task UnsubscribeDeviceAsync(Guid deviceId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"device:{deviceId}");
    }
}
