using System;

namespace CMMS.DAL.DTOs.IotDevices
{
    public class IotDeviceRequest
    {
        public Guid? BedId { get; set; }
        public string? DeviceCode { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? InstallationDate { get; set; }
        public double? Latitude { get; set; }
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
    }
}
