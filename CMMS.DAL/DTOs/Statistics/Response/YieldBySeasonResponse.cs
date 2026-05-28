using System;

namespace CMMS.DAL.DTOs.Statistics.Response
{
    public class YieldBySeasonResponse
    {
        public Guid SeasonId { get; set; }
        public string? SeasonName { get; set; }
        public DateOnly? SeasonStartDate { get; set; }
        public DateOnly? SeasonEndDate { get; set; }
        public int HarvestCount { get; set; }
        public decimal TotalActualWeightKg { get; set; }
        public int TotalActualQuantity { get; set; }
        public decimal TotalExpectedQuantity { get; set; }
        public decimal? FulfillmentRate { get; set; }
        public int CropsCovered { get; set; }
        public int PlotsCovered { get; set; }
    }
}
