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
            StageDescription = s.StageDescription,
            TemperatureMax = s.TemperatureMax,
            HumidityMax = s.HumidityMax,
            SoilMoistureMax = s.SoilMoistureMax,
            GrowthIndicators = s.GrowthIndicators,
            CommonDiseases = s.CommonDiseases,
            Notes = s.Notes,
            CreatedAt = s.CreatedAt
        };

        public static IEnumerable<CropGrowthStageResponse> ToResponseList(IEnumerable<CropGrowthStage> list)
            => list.Select(ToResponse);
    }
}
