using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs;

[Authorize]
public class PaymentHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetUserId();
        if (userId != Guid.Empty)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        await base.OnConnectedAsync();
    }
}
