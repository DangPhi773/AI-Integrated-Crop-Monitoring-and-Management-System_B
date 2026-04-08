using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities
{
    [Table("IotDevice")]
    public class IotDevice
    {
        [Key]
        public Guid DeviceId { get; set; } = Guid.NewGuid();

        public Guid? BedId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? Type { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? InstallationDate { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("BedId")]
        public virtual Bed? Bed { get; set; }

        public virtual ICollection<IotData> IotDatas { get; set; } = new List<IotData>();
    }
}