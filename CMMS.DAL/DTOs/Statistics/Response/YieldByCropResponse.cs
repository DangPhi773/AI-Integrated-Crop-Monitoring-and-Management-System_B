using System;

namespace CMMS.DAL.DTOs.Statistics.Response
{
    public class YieldByCropResponse
    {
        public Guid CropId { get; set; }
        public string CropName { get; set; } = string.Empty;
        public int HarvestCount { get; set; }
        public int CompletedDetailsCount { get; set; }
        public decimal TotalActualWeightKg { get; set; }
        public int TotalActualQuantity { get; set; }
        public decimal TotalExpectedQuantity { get; set; }
        public decimal AvgWeightKgPerHarvest { get; set; }
        public decimal? FulfillmentRate { get; set; }
        public int SeasonsCovered { get; set; }
    }
}
