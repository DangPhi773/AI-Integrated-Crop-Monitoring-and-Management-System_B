using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

public class ReportEnvironmentSnapshot
{
    [Key]
    public Guid Id { get; set; }

    public Guid ReportId { get; set; }

    public double? Temperature { get; set; }

    public double? Humidity { get; set; }

    public double? SoilMoisture { get; set; }

    public double? Rainfall { get; set; }

    public double? LightIntensity { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime RecordedAt { get; set; }

    public Guid? SourceDeviceId { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ReportId")]
    public virtual Report? Report { get; set; }

    [ForeignKey("SourceDeviceId")]
    public virtual IotDevice? SourceDevice { get; set; }
}
