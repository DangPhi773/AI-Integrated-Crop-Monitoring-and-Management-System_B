using CMMS.BLL.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs.Publishers;

public class IotRealtimePublisher : IIotRealtime
{
    private readonly IHubContext<IotHub> _hub;

    public IotRealtimePublisher(IHubContext<IotHub> hub)
        => _hub = hub;

    public Task PushSensorDataAsync(Guid farmId, Guid deviceId, object payload)
        => _hub.Clients.Groups($"farm:{farmId}", $"device:{deviceId}")
                       .SendAsync("SensorData", payload);

    public Task PushDeviceStatusAsync(Guid farmId, Guid deviceId, string status)
        => _hub.Clients.Groups($"farm:{farmId}", $"device:{deviceId}")
                       .SendAsync("DeviceStatus", new { deviceId, status });
}
