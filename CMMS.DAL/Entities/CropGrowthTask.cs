using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    public class CropGrowthTask
    {
        public Guid GrowthTaskId { get; set; }
        public Guid StageId { get; set; } 
        public string TaskName { get; set; }
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual CropGrowthStage CropGrowthStage { get; set; }
    }
}
