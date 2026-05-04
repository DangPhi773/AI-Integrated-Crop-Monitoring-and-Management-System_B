using CMMS.BLL.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace CMMS.WebAPI.Hubs.Publishers;

public class TaskRealtimePublisher : ITaskRealtime
{
    private readonly IHubContext<TaskHub> _hub;

    public TaskRealtimePublisher(IHubContext<TaskHub> hub)
        => _hub = hub;

    public Task PushTaskUpdatedAsync(Guid farmId, object payload)
        => _hub.Clients.Group($"farm:{farmId}").SendAsync("TaskUpdated", payload);

    public Task PushScheduleUpdatedAsync(Guid workerId, object payload)
        => _hub.Clients.Group($"user:{workerId}").SendAsync("ScheduleUpdated", payload);
}
