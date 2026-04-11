using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class CropGrowthStageMapper
    {
        public static CropGrowthStageResponse ToResponse(CropGrowthStage s) => new()
        {
            StageId = s.StageId,
            CropId = s.CropId,
            CropName = s.Crop?.CropName,
            StageName = s.StageName,
            TemperatureMin = s.TemperatureMin,
            CreatedAt = s.CreatedAt
        };
    }
}
