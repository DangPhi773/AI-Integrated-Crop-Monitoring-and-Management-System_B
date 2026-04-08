using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities
{
    [Table("SoilCropCompatibility")]
    public class SoilCropCompatibility
    {
        [Key]
        public Guid ComptId { get; set; }

        [Required]
        public Guid SoilId { get; set; }

        [Required]
        public Guid CropId { get; set; }

        [MaxLength(200)]
        public string? Compatibility { get; set; } 

        public string? Note { get; set; }

        [ForeignKey("SoilId")]
        public virtual Soil Soil { get; set; } = null!;

        [ForeignKey("CropId")]
        public virtual Crop Crop { get; set; } = null!;
    }
}