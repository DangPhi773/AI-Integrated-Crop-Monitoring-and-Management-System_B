using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Season")]
public partial class Season
{
    [Key]
    public Guid SeasonId { get; set; }

    public Guid? FarmId { get; set; }

    [MaxLength(200)]
    public string? SeasonName { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? SeasonStartDate { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? SeasonEndDate { get; set; }

    public string? Description { get; set; }

    public string? SeasonNotes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? SeasonCreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string? Status { get; set; }

    [ForeignKey("FarmId")]
    public virtual Farm? Farm { get; set; }

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
    public virtual ICollection<SeasonsDetail> SeasonsDetails { get; set; } = new List<SeasonsDetail>();
    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();
}