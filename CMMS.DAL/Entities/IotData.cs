using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities
{
    public class IotData
    {
        [Key]
        public Guid SensorDataId { get; set; } = Guid.NewGuid();

        public Guid? DeviceId { get; set; }
        public Guid? SeasonId { get; set; }
        public Guid? SensorId { get; set; }

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

        [Column(TypeName = "jsonb")]
        public string? RawData { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("DeviceId")]
        public virtual IotDevice? Device { get; set; }

        [ForeignKey("SeasonId")]
        public virtual Season? Season { get; set; }

        [ForeignKey("SensorId")]
        public virtual IotSensor? Sensor { get; set; }
    }
}
