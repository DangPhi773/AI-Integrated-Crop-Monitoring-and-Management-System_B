using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Interfaces
{
    public interface IAlertRuleEngine
    {
        System.Threading.Tasks.Task EvaluateAndNotifyAsync(
            IotDevice device,
            CropGrowthStage stage,
            SensorDataRequest reading,
            Guid? sensorDataId,
            DateTime recordedAt);
    }
}
