using CMMS.DAL.DTOs.IotDevices;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class IotDeviceMapper
    {
        public static IotDeviceResponse ToResponse(IotDevice d) => new()
        {
            DeviceId = d.DeviceId,
            BedId = d.BedId,
            DeviceCode = d.DeviceCode,
            Name = d.Name,
            Type = d.Type,
            Status = d.Status,
            InstallationDate = d.InstallationDate,
            Latitude = d.Latitude,
            Longitude = d.Longitude,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt,
            LastActiveAt = d.LastActiveAt
        };
    }
}
