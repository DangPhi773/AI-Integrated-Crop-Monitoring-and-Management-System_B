using System;

namespace CMMS.DAL.DTOs.Statistics.Request
{
    public class YieldStatsFilter
    {
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
        public Guid? FarmId { get; set; }
        public Guid? CropId { get; set; }
        public Guid? SeasonId { get; set; }
    }
}
