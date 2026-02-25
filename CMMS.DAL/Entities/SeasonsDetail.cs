using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class SeasonsDetail
{
    public Guid SeasonDetailId { get; set; }

    public Guid? SeasonId { get; set; }

    public Guid? BedId { get; set; }

    public Guid? CropId { get; set; }

    public DateOnly? SeasonExpectedHarvestDate { get; set; }

    public int? CropQuantity { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? TotalHarvestYield { get; set; }

    public virtual Bed? Bed { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual Season? Season { get; set; }
}
