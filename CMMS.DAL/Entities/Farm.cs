using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Farm")]
public partial class Farm
{
    [Key]
    public Guid FarmId { get; set; }

    [Required(ErrorMessage = "Tên trang trại là bắt buộc")]
    [StringLength(255)]
    public string FarmName { get; set; } = null!;

    [StringLength(500)]
    public string? FarmLocation { get; set; }

    [Column(TypeName = "decimal(10,7)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(10,7)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? FarmArea { get; set; }

    [StringLength(50)]
    public string? FarmStatus { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? FarmCreatedAt { get; set; }

    public virtual ICollection<Plot> Plots { get; set; } = new List<Plot>();

    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
}
