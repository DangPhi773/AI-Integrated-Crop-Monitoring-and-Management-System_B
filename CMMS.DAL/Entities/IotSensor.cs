using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class IotSensor
{
    [Key]
    public Guid Id { get; set; }

    public Guid DeviceId { get; set; }

    [Required]
    [MaxLength(50)]
    public string SensorCode { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string SensorName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string SensorType { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = null!;

    public double? MinValue { get; set; }

    public double? MaxValue { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "ACTIVE";

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("DeviceId")]
    public virtual IotDevice? Device { get; set; }

    public virtual ICollection<IotData> IotDatas { get; set; } = new List<IotData>();
}
