using System;

namespace CMMS.DAL.DTOs.Seasons
{
    public class SeasonRequest
    {
        public Guid? FarmId { get; set; }
        public string? SeasonName { get; set; }
        public DateOnly? SeasonStartDate { get; set; }
        public DateOnly? SeasonEndDate { get; set; }
        public string? Description { get; set; }
        public string? SeasonNotes { get; set; }
        public string? Status { get; set; }
    }

    public class SeasonResponse
    {
        public Guid SeasonId { get; set; }
        public Guid? FarmId { get; set; }
        public string? SeasonName { get; set; }
        public DateOnly? SeasonStartDate { get; set; }
        public DateOnly? SeasonEndDate { get; set; }
        public string? Description { get; set; }
        public string? SeasonNotes { get; set; }
        public DateTime? SeasonCreatedAt { get; set; }
        public string? Status { get; set; }

        public int HarvestsCount { get; set; }
        public int TasksCount { get; set; }
    }
}
