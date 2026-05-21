using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.IotDevices
{
    public class IotDeviceRequest
    {
        public Guid? BedId { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "DeviceCode phải từ 1 đến 100 ký tự")]
        public string? DeviceCode { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessage = "Name phải từ 1 đến 150 ký tự")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "Type tối đa 50 ký tự")]
        public string? Type { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
        public string? Status { get; set; }

        public DateTime? InstallationDate { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = "Latitude phải từ -90 đến 90")]
        public double? Latitude { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = "Longitude phải từ -180 đến 180")]
        public double? Longitude { get; set; }
    }

    public class IotDeviceResponse
    {
        public Guid DeviceId { get; set; }
        public Guid? BedId { get; set; }
        public string? DeviceCode { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? InstallationDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastActiveAt { get; set; }
        public bool HasApiKey { get; set; }
        public DateTime? ApiKeyRotatedAt { get; set; }
    }

    public class IotDeviceCreatedResponse
    {
        public Guid DeviceId { get; set; }
        public string? DeviceCode { get; set; }
        public string? Name { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string Warning { get; set; } = "Hãy lưu ApiKey này ngay. Khoá sẽ không hiển thị lại. Nếu mất, hãy rotate-key để sinh khoá mới.";
    }
}
