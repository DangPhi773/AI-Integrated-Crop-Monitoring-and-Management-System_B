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
            Temperature = d.Temperature,
            Humidity = d.Humidity,
            SoilMoisture = d.SoilMoisture,
            Light = d.Light,
            IsRaining = d.IsRaining,
            IsAlert = d.IsAlert,
            CreatedAt = d.CreatedAt
        };
    }
}
