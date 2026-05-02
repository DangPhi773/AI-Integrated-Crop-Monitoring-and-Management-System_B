using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("HarvestRecord")]
public partial class HarvestRecord
{
    [Key]
    public Guid HarvestRecordId { get; set; }

    [Required]
    public Guid HarvestId { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateOnly HarvestDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "date")]
    public DateOnly? SaleDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? SoldQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalAmount { get; set; }

    [MaxLength(200)]
    public string? BuyerName { get; set; }

    [MaxLength(50)]
    public string? SaleChannel { get; set; }

    public string? Notes { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("HarvestId")]
    public virtual Harvest Harvest { get; set; } = null!;
}
