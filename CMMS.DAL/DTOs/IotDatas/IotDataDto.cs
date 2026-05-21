using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.IotDatas
{
    public class IotDataRequest
    {
        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }

        [Range(-50.0, 80.0, ErrorMessage = "Nhiệt độ phải từ -50 đến 80 °C")]
        public double? Temperature { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Độ ẩm phải từ 0 đến 100 %")]
        public double? Humidity { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Độ ẩm đất phải từ 0 đến 100 %")]
        public double? SoilMoisture { get; set; }

        [Range(0.0, 200000.0, ErrorMessage = "Ánh sáng phải từ 0 đến 200,000 lux")]
        public double? Light { get; set; }

        public bool? IsRaining { get; set; }
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
        [Range(-50.0, 80.0, ErrorMessage = "Nhiệt độ phải từ -50 đến 80 °C")]
        public double? Temperature { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Độ ẩm phải từ 0 đến 100 %")]
        public double? Humidity { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Độ ẩm đất phải từ 0 đến 100 %")]
        public double? SoilMoisture { get; set; }

        [Range(0.0, 200000.0, ErrorMessage = "Ánh sáng phải từ 0 đến 200,000 lux")]
        public double? Light { get; set; }

        public bool? IsRaining { get; set; }

        [StringLength(100, ErrorMessage = "DeviceId tối đa 100 ký tự")]
        public string? DeviceId { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class SensorDataResponse
    {
        public Guid Id { get; set; }
        public bool IsAlert { get; set; }
        public string Message { get; set; } = null!;
    }
}
