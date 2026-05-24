using System;

namespace CMMS.DAL.DTOs.Statistics.Response
{
    public class YieldByPlotResponse
    {
        public Guid PlotId { get; set; }
        public string? PlotName { get; set; }
        public decimal? PlotArea { get; set; }
        public int HarvestCount { get; set; }
        public decimal TotalActualWeightKg { get; set; }
        public decimal? YieldPerAreaKg { get; set; }
        public int CropsCovered { get; set; }
    }
}
