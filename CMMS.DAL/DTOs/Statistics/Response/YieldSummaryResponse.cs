using System;

namespace CMMS.DAL.DTOs.Statistics.Response
{
    public class YieldSummaryResponse
    {
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
        public int TotalHarvests { get; set; }
        public int CompletedHarvests { get; set; }
        public int OngoingHarvests { get; set; }
        public decimal TotalActualWeightKg { get; set; }
        public decimal TotalExpectedQuantity { get; set; }
        public decimal? OverallFulfillmentRate { get; set; }
        public int CropsCount { get; set; }
        public int SeasonsCount { get; set; }
        public int PlotsCount { get; set; }
        public Guid? TopCropId { get; set; }
        public string? TopCropName { get; set; }
        public decimal TopCropWeightKg { get; set; }
    }
}
