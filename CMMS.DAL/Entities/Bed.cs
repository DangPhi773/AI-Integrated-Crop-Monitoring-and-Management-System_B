using System;
using System.Collections.Generic;

namespace CMMS.DAL.Entities;

public partial class Bed
{
    public Guid BedId { get; set; }

    public Guid? PlotId { get; set; }

    public string? BedName { get; set; }

    public decimal? BedArea { get; set; }

    public string? BedStatus { get; set; }

    public DateTime? BedCreatedAt { get; set; }

    public int? CropQuantities { get; set; }

    public string? PlantingPattern { get; set; }

    public int? RowCount { get; set; }

    public double? BedWidth { get; set; }

    public double? BedLength { get; set; }

    public virtual Plot? Plot { get; set; }

    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();
    // Thêm bộ sưu tập các thiết bị IoT vào luống (Bed)
    public virtual ICollection<IotDevice> IotDevices { get; set; } = new List<IotDevice>();
}
