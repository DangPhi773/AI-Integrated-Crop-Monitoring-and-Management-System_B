using System;

namespace CMMS.DAL.DTOs.IotDatas
{
    public class IotDataRequest
    {
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? Light { get; set; }
        public bool? IsRaining { get; set; }
        public bool IsAlert { get; set; }
    }

    public class IotDataResponse
    {
        public Guid SensorDataId { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }
        public DateTime? RecordedAt { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? Light { get; set; }
        public bool? IsRaining { get; set; }
        public bool IsAlert { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SensorDataRequest
    {
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? Light { get; set; }
        public bool? IsRaining { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }

    public class SensorDataResponse
    {
        public Guid Id { get; set; }
        public bool IsAlert { get; set; }
        public string Message { get; set; } = null!;
    }
}
