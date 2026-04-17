using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class CropMapper
    {
        public static CropResponse ToResponse(Crop c) => new()
        {
            CropId = c.CropId,
            CropName = c.CropName,
            CropScientificName = c.CropScientificName,
            CropDefaultGrowthDays = c.CropDefaultGrowthDays,
            PlantSpacing = c.PlantSpacing,
            BedWidthDefault = c.BedWidthDefault,
            PathWidthDefault = c.PathWidthDefault,
            RowsPerBed = c.RowsPerBed,
            RowSpacing = c.RowSpacing,
            CropQuantities = c.CropQuantities,
            CropStatus = c.CropStatus,
            CompatibleSoils = c.SoilCropCompatibilities?.Select(sc => new SoilCompatibilityDto
            {
                SoilId = sc.SoilId,
                SoilName = sc.Soil?.Name,
                Compatibility = sc.Compatibility
            }).ToList() ?? new List<SoilCompatibilityDto>()
        };
    }
}
