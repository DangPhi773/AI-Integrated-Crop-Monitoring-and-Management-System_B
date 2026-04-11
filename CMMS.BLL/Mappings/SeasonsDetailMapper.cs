using CMMS.DAL.DTOs.SeasonsDetails;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class SeasonsDetailMapper
    {
        public static SeasonsDetailResponse ToResponse(SeasonsDetail sd) => new()
        {
            SeasonDetailId = sd.SeasonDetailId,
            SeasonId = sd.SeasonId,
            BedId = sd.BedId,
            CropId = sd.CropId,
            CropQuantity = sd.CropQuantity,
            StartDate = sd.StartDate,
            EndDate = sd.EndDate,
            SeasonExpectedHarvestDate = sd.SeasonExpectedHarvestDate,
            TotalHarvestYield = sd.TotalHarvestYield,
            SeasonName = sd.Season?.SeasonName,
            BedName = sd.Bed?.BedName,
            CropName = sd.Crop?.CropName,
            PhotosCount = 0
        };
    }
}
