using CMMS.BLL.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs.Publishers;

public class PaymentRealtimePublisher : IPaymentRealtime
{
    private readonly IHubContext<PaymentHub> _hub;

    public PaymentRealtimePublisher(IHubContext<PaymentHub> hub)
        => _hub = hub;

    public Task PushPaymentStatusAsync(Guid userId, object payload)
        => _hub.Clients.Group($"user:{userId}").SendAsync("PaymentStatus", payload);
}
