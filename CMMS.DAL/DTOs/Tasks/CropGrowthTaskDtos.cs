using System;
using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Tasks
{
    public class CropGrowthTaskRequest
    {
        [Required(ErrorMessage = "StageId là bắt buộc")]
        public Guid StageId { get; set; }

        public Guid? TaskId { get; set; }

        [StringLength(1000, ErrorMessage = "TaskDescription tối đa 1000 ký tự")]
        public string? TaskDescription { get; set; }

        [StringLength(100, ErrorMessage = "Frequency tối đa 100 ký tự")]
        public string? Frequency { get; set; }

        [Range(0, 100000, ErrorMessage = "DurationMinutes phải từ 0 đến 100,000")]
        public int? DurationMinutes { get; set; }

        [StringLength(500, ErrorMessage = "RequiredTools tối đa 500 ký tự")]
        public string? RequiredTools { get; set; }

        [StringLength(500, ErrorMessage = "RequiredMaterials tối đa 500 ký tự")]
        public string? RequiredMaterials { get; set; }

        [Range(0.0, 1000000, ErrorMessage = "QuantityPerUnit phải từ 0 đến 1,000,000")]
        public double? QuantityPerUnit { get; set; }

        [StringLength(50, ErrorMessage = "QuantityUnit tối đa 50 ký tự")]
        public string? QuantityUnit { get; set; }

        [Range(0, 10, ErrorMessage = "Priority phải từ 0 đến 10")]
        public int Priority { get; set; }

        public bool IsMandatory { get; set; }

        [StringLength(1000, ErrorMessage = "Notes tối đa 1000 ký tự")]
        public string? Notes { get; set; }
    }

    public class CropGrowthTaskResponse
    {
        public Guid GrowthTaskId { get; set; }
        public Guid StageId { get; set; }
        public string? StageName { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        public string? Frequency { get; set; }
        public int? DurationMinutes { get; set; }
        public string? RequiredTools { get; set; }
        public string? RequiredMaterials { get; set; }
        public double? QuantityPerUnit { get; set; }
        public string? QuantityUnit { get; set; }
        public int Priority { get; set; }
        public bool IsMandatory { get; set; }
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
