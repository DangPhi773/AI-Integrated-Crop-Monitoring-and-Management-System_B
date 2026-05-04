namespace CMMS.BLL.Realtime;

public interface IIotRealtime
{
    Task PushSensorDataAsync(Guid farmId, Guid deviceId, object payload);
    Task PushDeviceStatusAsync(Guid farmId, Guid deviceId, string status);
}
