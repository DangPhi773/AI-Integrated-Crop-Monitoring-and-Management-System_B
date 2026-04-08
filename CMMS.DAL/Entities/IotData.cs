using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities
{
    [Table("IotData")]
    public class IotData
    {
        [Key]
        public Guid SensorDataId { get; set; } = Guid.NewGuid();

        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }

        [Column(TypeName = "timestamp")] 
        public DateTime? RecordedAt { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; } 

        public double? Value { get; set; } 

        [MaxLength(20)]
        public string? Unit { get; set; }

        public bool? IsAlert { get; set; } 

        public double? Min { get; set; } 
        public double? Max { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DeviceId")]
        public virtual IotDevice? Device { get; set; }

        [ForeignKey("SeasonId")]
        public virtual Season? Season { get; set; }
    }
}