using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.SeasonsDetails
{
    public class SeasonsDetailRequest
    {
        public Guid? SeasonId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? CropId { get; set; }
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? SeasonExpectedHarvestDate { get; set; }
        public decimal? TotalHarvestYield { get; set; }
    }

    public class SeasonsDetailResponse
    {
        public Guid SeasonDetailId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? BedId { get; set; }
        public Guid? CropId { get; set; }
        public int? CropQuantity { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? SeasonExpectedHarvestDate { get; set; }
        public decimal? TotalHarvestYield { get; set; }

        public string? SeasonName { get; set; }
        public string? BedName { get; set; }
        public string? CropName { get; set; }
        public int PhotosCount { get; set; } = 0;
    }
}
