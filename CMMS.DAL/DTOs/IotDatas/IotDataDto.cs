using System;

namespace CMMS.DAL.DTOs.IotDatas
{
    public class IotDataRequest
    {
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }
        public string? Type { get; set; }
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public bool? IsAlert { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
    }

    public class IotDataResponse
    {
        public Guid SensorDataId { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }
        public DateTime? RecordedAt { get; set; }
        public string? Type { get; set; }
        public double? Value { get; set; }
        public string? Unit { get; set; }
        public bool? IsAlert { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
