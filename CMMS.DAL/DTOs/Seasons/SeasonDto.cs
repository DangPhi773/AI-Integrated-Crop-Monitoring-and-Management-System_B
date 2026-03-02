using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int SeasonsDetailsCount { get; set; }
        public int TasksCount { get; set; }

        public List<SeasonsDetailDto> SeasonsDetails { get; set; } = new();
    }

    public class SeasonsDetailDto
    {
        public Guid SeasonDetailId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? CropId { get; set; }
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? SeasonExpectedHarvestDate { get; set; }
        public decimal? TotalHarvestYield { get; set; }
    }
}
