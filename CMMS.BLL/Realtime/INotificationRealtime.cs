namespace CMMS.BLL.Realtime;

public interface INotificationRealtime
{
    Task PushToUserAsync(Guid userId, object payload);
}
