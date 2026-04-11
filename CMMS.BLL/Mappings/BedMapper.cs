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
            PlotName = b.Plot?.PlotName,
            SeasonsDetailsCount = b.SeasonsDetails?.Count ?? 0
        };
    }
}
