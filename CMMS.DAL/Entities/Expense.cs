using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("Expense")]
public class Expense
{
    [Key]
    public Guid ExpenseId { get; set; }

    [Required]
    public Guid SeasonId { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = null!;

    [Required, MaxLength(255)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(14,2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "date")]
    public DateOnly SpentAt { get; set; }

    public string? Notes { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("SeasonId")] public virtual Season? Season { get; set; }
    [ForeignKey("CreatedBy")] public virtual User? Creator { get; set; }
}
