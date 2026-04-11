using CMMS.DAL.DTOs.CropBedConfigs;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class CropBedConfigMapper
    {
        public static CropBedConfigResponse ToResponse(CropBedConfig c) => new()
        {
            ConfigId = c.ConfigId,
            CropId = c.CropId,
            CropName = c.Crop?.CropName,
            PlantingPattern = c.PlantingPattern,
            RowSpacing = c.RowSpacing,
            PlantSpacing = c.PlantSpacing,
            RowsPerBed = c.RowsPerBed,
            BedWidthMin = c.BedWidthMin,
            BedWidthMax = c.BedWidthMax,
            PathWidthMin = c.PathWidthMin,
            PathWidthMax = c.PathWidthMax,
            BedHeight = c.BedHeight,
            DensityPerHaMin = c.DensityPerHaMin,
            DensityPerHaMax = c.DensityPerHaMax,
            IsDefault = c.IsDefault,
            Notes = c.Notes,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
    }
}
