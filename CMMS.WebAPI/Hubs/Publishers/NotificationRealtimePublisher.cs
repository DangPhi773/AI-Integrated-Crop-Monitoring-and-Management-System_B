using CMMS.BLL.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs.Publishers;

public class NotificationRealtimePublisher : INotificationRealtime
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationRealtimePublisher(IHubContext<NotificationHub> hub)
        => _hub = hub;

    public Task PushToUserAsync(Guid userId, object payload)
        => _hub.Clients.Group($"user:{userId}").SendAsync("Notification", payload);
}
