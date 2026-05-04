namespace CMMS.BLL.Realtime;

public interface ITaskRealtime
{
    Task PushTaskUpdatedAsync(Guid farmId, object payload);
    Task PushScheduleUpdatedAsync(Guid workerId, object payload);
}
