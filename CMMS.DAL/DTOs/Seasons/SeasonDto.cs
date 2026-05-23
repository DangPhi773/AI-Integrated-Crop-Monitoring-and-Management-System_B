using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Seasons
{
    public class SeasonRequest
    {
        public Guid? FarmId { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên mùa vụ phải từ 1 đến 150 ký tự")]
        public string? SeasonName { get; set; }

        public DateOnly? SeasonStartDate { get; set; }
        public DateOnly? SeasonEndDate { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? Description { get; set; }

        [StringLength(1000, ErrorMessage = "SeasonNotes tối đa 1000 ký tự")]
        public string? SeasonNotes { get; set; }

        [StringLength(50, ErrorMessage = "Status tối đa 50 ký tự")]
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
