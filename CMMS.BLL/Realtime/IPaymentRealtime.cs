namespace CMMS.BLL.Realtime;

public interface IPaymentRealtime
{
    Task PushPaymentStatusAsync(Guid userId, object payload);
}
