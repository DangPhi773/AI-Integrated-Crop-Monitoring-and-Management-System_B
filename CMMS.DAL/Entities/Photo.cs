using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Photo
{
    public Guid PhotoId { get; set; }

    public Guid? SeasonDetailId { get; set; }

    public DateOnly? PhotoDate { get; set; }

    public TimeOnly? PhotoTime { get; set; }

    public decimal? TemperatureAtTheMomentTakePhoto { get; set; }

    public decimal? HumidityAtTheMomentTakePhoto { get; set; }

    public decimal? SoilMoistureAtTheMomentTakePhoto { get; set; }

    public decimal? RainfallAtTheMomentTakePhoto { get; set; }

    public string? PhotoSource { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<ImageAnalysis> ImageAnalyses { get; set; } = new List<ImageAnalysis>();

    public virtual SeasonsDetail? SeasonDetail { get; set; }
}
