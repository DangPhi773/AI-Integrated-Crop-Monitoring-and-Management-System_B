using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Photo")]
public partial class Photo
{
    [Key]
    public Guid PhotoId { get; set; }

    public Guid? SeasonDetailId { get; set; }

    public DateOnly? PhotoDate { get; set; }

    public TimeOnly? PhotoTime { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TemperatureAtTheMomentTakePhoto { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? HumidityAtTheMomentTakePhoto { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? SoilMoistureAtTheMomentTakePhoto { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? RainfallAtTheMomentTakePhoto { get; set; }

    [MaxLength(500)]
    public string? PhotoSource { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? UploadedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ImageAnalysis> ImageAnalyses { get; set; } = new List<ImageAnalysis>();

    [ForeignKey("SeasonDetailId")]
    public virtual SeasonsDetail? SeasonDetail { get; set; }
}