using System;

namespace CMMS.DAL.DTOs.Harvests
{
    public class CreateHarvestRecordRequest
    {
        public Guid HarvestId { get; set; }
        public DateOnly HarvestDate { get; set; }
        public decimal Quantity { get; set; }
        public DateOnly? SaleDate { get; set; }
        public decimal? SoldQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? BuyerName { get; set; }
        public string? SaleChannel { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateHarvestRecordRequest
    {
        public DateOnly? HarvestDate { get; set; }
        public decimal? Quantity { get; set; }
        public DateOnly? SaleDate { get; set; }
        public decimal? SoldQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? BuyerName { get; set; }
        public string? SaleChannel { get; set; }
        public string? Notes { get; set; }
    }

    public class RecordSaleRequest
    {
        public DateOnly SaleDate { get; set; }
        public decimal SoldQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? BuyerName { get; set; }
        public string? SaleChannel { get; set; }
        public string? Notes { get; set; }
    }

    public class HarvestRecordResponse
    {
        public Guid HarvestRecordId { get; set; }
        public Guid HarvestId { get; set; }
        public DateOnly HarvestDate { get; set; }
        public decimal Quantity { get; set; }
        public DateOnly? SaleDate { get; set; }
        public decimal? SoldQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? BuyerName { get; set; }
        public string? SaleChannel { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsSold => SaleDate.HasValue;
    }
}
