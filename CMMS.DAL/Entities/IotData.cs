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

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? RecordedAt { get; set; }

        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? SoilMoisture { get; set; }
        public double? Light { get; set; }
        public bool? IsRaining { get; set; }

        public bool IsAlert { get; set; } = false;

        [Column(TypeName = "jsonb")]
        public string? RawData { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("DeviceId")]
        public virtual IotDevice? Device { get; set; }

        [ForeignKey("SeasonId")]
        public virtual Season? Season { get; set; }
    }
}
