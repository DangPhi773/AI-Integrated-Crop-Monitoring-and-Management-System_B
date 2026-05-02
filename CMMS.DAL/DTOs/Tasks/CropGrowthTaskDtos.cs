using System;

namespace CMMS.DAL.DTOs.Tasks
{
    public class CropGrowthTaskRequest
    {
        public Guid StageId { get; set; }
        public Guid? TaskId { get; set; }
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
