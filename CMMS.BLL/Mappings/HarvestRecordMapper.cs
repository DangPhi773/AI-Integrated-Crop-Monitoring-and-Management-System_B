using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class HarvestRecordMapper
    {
        public static HarvestRecordResponse ToResponse(HarvestRecord r) => new()
        {
            HarvestRecordId = r.HarvestRecordId,
            HarvestId = r.HarvestId,
            HarvestDate = r.HarvestDate,
            Quantity = r.Quantity,
            SaleDate = r.SaleDate,
            SoldQuantity = r.SoldQuantity,
            UnitPrice = r.UnitPrice,
            TotalAmount = r.TotalAmount,
            BuyerName = r.BuyerName,
            SaleChannel = r.SaleChannel,
            Notes = r.Notes,
            CreatedAt = r.CreatedAt
        };
    }
}
