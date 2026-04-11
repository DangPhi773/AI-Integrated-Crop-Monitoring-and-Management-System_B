using CMMS.DAL.DTOs.Farms;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class FarmMapper
    {
        public static FarmResponse ToResponse(Farm f) => new()
        {
            FarmId = f.FarmId,
            FarmName = f.FarmName,
            FarmLocation = f.FarmLocation,
            FarmArea = f.FarmArea,
            FarmStatus = f.FarmStatus,
            FarmCreatedAt = f.FarmCreatedAt,
            SeasonsCount = f.Seasons?.Count ?? 0
        };
    }
}
