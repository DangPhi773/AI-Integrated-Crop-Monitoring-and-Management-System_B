using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class BedMapper
    {
        public static BedResponse ToResponse(Bed b) => new()
        {
            BedId = b.BedId,
            PlotId = b.PlotId,
            BedName = b.BedName,
            BedArea = b.BedArea,
            BedStatus = b.BedStatus,
            BedCreatedAt = b.BedCreatedAt,
            CropQuantities = b.CropQuantities,
            BedWidth = b.BedWidth,
            BedLength = b.BedLength,
            PathWidth = b.PathWidth,
            PlantCount = b.PlantCount,
            RowCount = b.RowCount,
            PlotName = b.Plot?.PlotName,
            HarvestDetailsCount = b.HarvestDetails?.Count ?? 0
        };
    }
}
