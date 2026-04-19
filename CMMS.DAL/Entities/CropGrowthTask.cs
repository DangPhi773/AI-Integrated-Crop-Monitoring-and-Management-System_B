using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Entities
{
    [Table("CropGrowthTask")]
    public class CropGrowthTask
    {
        [Key]
        public Guid GrowthTaskId { get; set; }
        public Guid StageId { get; set; }

        [Required]
        [MaxLength(255)]
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; }

        [MaxLength(100)]
        public string? Frequency { get; set; }
        public int? DurationMinutes { get; set; }
        public string? RequiredTools { get; set; }
        public string? RequiredMaterials { get; set; }
        public double? QuantityPerUnit { get; set; }

        [MaxLength(50)]
        public string? QuantityUnit { get; set; }
        public int Priority { get; set; }
        public bool IsMandatory { get; set; }
        public string? Notes { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("StageId")]
        public virtual CropGrowthStage CropGrowthStage { get; set; }
    }
}
