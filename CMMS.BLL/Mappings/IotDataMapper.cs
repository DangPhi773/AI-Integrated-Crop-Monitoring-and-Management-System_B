using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class IotDataMapper
    {
        public static IotDataResponse ToResponse(IotData d) => new()
        {
            SensorDataId = d.SensorDataId,
            DeviceId = d.DeviceId,
            SeasonId = d.SeasonId,
            RecordedAt = d.RecordedAt,
            Type = d.Type,
            Value = d.Value,
            Unit = d.Unit,
            IsAlert = d.IsAlert,
            Min = d.Min,
            Max = d.Max,
            CreatedAt = d.CreatedAt
        };
    }
}
