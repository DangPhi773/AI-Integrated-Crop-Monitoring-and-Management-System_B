using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class HarvestRecordService : IHarvestRecordService
    {
        private readonly IHarvestRecordRepository _repo;
        private readonly IHarvestRepository _harvestRepo;

        public HarvestRecordService(IHarvestRecordRepository repo, IHarvestRepository harvestRepo)
        {
            _repo = repo;
            _harvestRepo = harvestRepo;
        }

        public async Task<ApiResponse<HarvestRecordResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Không tìm thấy record" };
                return new ApiResponse<HarvestRecordResponse> { Success = true, Data = HarvestRecordMapper.ToResponse(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<HarvestRecordResponse>>> GetByHarvestIdAsync(Guid harvestId)
        {
            try
            {
                var items = await _repo.GetByHarvestIdAsync(harvestId);
                return new ApiResponse<IEnumerable<HarvestRecordResponse>>
                {
                    Success = true,
                    Data = items.Select(HarvestRecordMapper.ToResponse)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<HarvestRecordResponse>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestRecordResponse>> CreateAsync(CreateHarvestRecordRequest request)
        {
            try
            {
                if (request.Quantity < 0)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Quantity phải >= 0" };
                if (request.SoldQuantity.HasValue && request.SoldQuantity.Value > request.Quantity)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "SoldQuantity không được lớn hơn Quantity" };

                var harvest = await _harvestRepo.GetByIdAsync(request.HarvestId);
                if (harvest == null)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Harvest không tồn tại" };

                var totalAmount = request.TotalAmount;
                if (!totalAmount.HasValue && request.SoldQuantity.HasValue && request.UnitPrice.HasValue)
                    totalAmount = request.SoldQuantity.Value * request.UnitPrice.Value;

                var entity = new HarvestRecord
                {
                    HarvestRecordId = Guid.NewGuid(),
                    HarvestId = request.HarvestId,
                    HarvestDate = request.HarvestDate,
                    Quantity = request.Quantity,
                    SaleDate = request.SaleDate,
                    SoldQuantity = request.SoldQuantity,
                    UnitPrice = request.UnitPrice,
                    TotalAmount = totalAmount,
                    BuyerName = request.BuyerName,
                    SaleChannel = request.SaleChannel,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(entity);
                if (!await _repo.SaveChangesAsync())
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Lưu thất bại" };

                return new ApiResponse<HarvestRecordResponse>
                {
                    Success = true,
                    Message = "Tạo harvest record thành công",
                    Data = HarvestRecordMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestRecordResponse>
                {
                    Success = false,
                    Message = "Lỗi tạo record",
                    Errors = new List<string> { ex.InnerException?.Message ?? ex.Message }
                };
            }
        }

        public async Task<ApiResponse<HarvestRecordResponse>> UpdateAsync(Guid id, UpdateHarvestRecordRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Không tìm thấy record" };

                if (request.HarvestDate.HasValue) entity.HarvestDate = request.HarvestDate.Value;
                if (request.Quantity.HasValue)
                {
                    if (request.Quantity.Value < 0)
                        return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Quantity phải >= 0" };
                    entity.Quantity = request.Quantity.Value;
                }
                if (request.SaleDate.HasValue) entity.SaleDate = request.SaleDate;
                if (request.SoldQuantity.HasValue) entity.SoldQuantity = request.SoldQuantity;
                if (request.UnitPrice.HasValue) entity.UnitPrice = request.UnitPrice;
                if (request.TotalAmount.HasValue) entity.TotalAmount = request.TotalAmount;
                if (request.BuyerName != null) entity.BuyerName = request.BuyerName;
                if (request.SaleChannel != null) entity.SaleChannel = request.SaleChannel;
                if (request.Notes != null) entity.Notes = request.Notes;

                if (entity.SoldQuantity.HasValue && entity.SoldQuantity.Value > entity.Quantity)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "SoldQuantity không được lớn hơn Quantity" };

                if (!entity.TotalAmount.HasValue && entity.SoldQuantity.HasValue && entity.UnitPrice.HasValue)
                    entity.TotalAmount = entity.SoldQuantity.Value * entity.UnitPrice.Value;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<HarvestRecordResponse>
                {
                    Success = true,
                    Message = "Cập nhật record thành công",
                    Data = HarvestRecordMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Lỗi cập nhật", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestRecordResponse>> RecordSaleAsync(Guid id, RecordSaleRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Không tìm thấy record" };

                if (request.SoldQuantity > entity.Quantity)
                    return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "SoldQuantity không được lớn hơn Quantity" };

                entity.SaleDate = request.SaleDate;
                entity.SoldQuantity = request.SoldQuantity;
                entity.UnitPrice = request.UnitPrice;
                entity.TotalAmount = request.SoldQuantity * request.UnitPrice;
                if (request.BuyerName != null) entity.BuyerName = request.BuyerName;
                if (request.SaleChannel != null) entity.SaleChannel = request.SaleChannel;
                if (request.Notes != null) entity.Notes = request.Notes;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<HarvestRecordResponse>
                {
                    Success = true,
                    Message = "Ghi nhận bán thành công",
                    Data = HarvestRecordMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestRecordResponse> { Success = false, Message = "Lỗi ghi nhận bán", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy record" };

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Đã xóa record" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi xóa", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
