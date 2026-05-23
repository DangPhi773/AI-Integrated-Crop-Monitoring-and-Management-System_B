using CMMS.DAL.DTOs.Plots;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class PlotMapper
    {
        public static PlotResponse ToResponse(Plot p) => new()
        {
            PlotId = p.PlotId,
            FarmId = p.FarmId,
            SoilId = p.SoilId,
            PlotName = p.PlotName,
            PlotArea = p.PlotArea,
            PlotLength = p.PlotLength,
            PlotWidth = p.PlotWidth,
            PlotMarginLength = p.PlotMarginLength,
            PlotMarginWidth = p.PlotMarginWidth,
            PlotStatus = p.PlotStatus,
            PlotCreatedAt = p.PlotCreatedAt,
            FarmName = p.Farm?.FarmName,
            SoilName = p.Soil?.Name,
            BedsCount = p.Beds?.Count ?? 0
        };
    }
}
