using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class HarvestDetailService : IHarvestDetailService
    {
        private readonly IHarvestDetailRepository _repo;
        private readonly IHarvestRepository _harvestRepo;

        public HarvestDetailService(IHarvestDetailRepository repo, IHarvestRepository harvestRepo)
        {
            _repo = repo;
            _harvestRepo = harvestRepo;
        }

        public async Task<ApiResponse<HarvestDetailResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Không tìm thấy harvest detail" };
                return new ApiResponse<HarvestDetailResponse> { Success = true, Data = HarvestDetailMapper.ToResponse(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<HarvestDetailResponse>>> GetByHarvestIdAsync(Guid harvestId)
        {
            try
            {
                var items = await _repo.GetByHarvestIdAsync(harvestId);
                return new ApiResponse<IEnumerable<HarvestDetailResponse>>
                {
                    Success = true,
                    Data = items.Select(HarvestDetailMapper.ToResponse)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<HarvestDetailResponse>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestDetailResponse>> UpdateAsync(Guid id, UpdateHarvestDetailRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Không tìm thấy harvest detail" };

                if (request.CropQuantity.HasValue) entity.CropQuantity = request.CropQuantity;
                if (request.StartDate.HasValue) entity.StartDate = request.StartDate;
                if (request.EndDate.HasValue) entity.EndDate = request.EndDate;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                return new ApiResponse<HarvestDetailResponse>
                {
                    Success = true,
                    Message = "Cập nhật harvest detail thành công",
                    Data = HarvestDetailMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Lỗi cập nhật", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestDetailResponse>> RecordHarvestAsync(Guid id, RecordHarvestRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Không tìm thấy harvest detail" };

                if (!request.ActualQuantity.HasValue && !request.ActualWeightKg.HasValue)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Cần ghi ít nhất số lượng hoặc khối lượng" };

                if (request.ActualQuantity.HasValue && request.ActualQuantity.Value < 0)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "ActualQuantity phải >= 0" };

                if (request.ActualWeightKg.HasValue && request.ActualWeightKg.Value < 0)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "ActualWeightKg phải >= 0" };

                if (entity.StartDate.HasValue && request.ActualHarvestDate < entity.StartDate.Value)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Ngày thu không thể trước ngày bắt đầu" };

                if (entity.CropQuantity.HasValue && request.ActualQuantity.HasValue && request.ActualQuantity.Value > entity.CropQuantity.Value)
                    return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "ActualQuantity không được lớn hơn CropQuantity" };

                entity.ActualHarvestDate = request.ActualHarvestDate;
                entity.ActualQuantity = request.ActualQuantity;
                entity.ActualWeightKg = request.ActualWeightKg;
                if (request.HarvestNotes != null) entity.HarvestNotes = request.HarvestNotes;

                _repo.Update(entity);
                await _repo.SaveChangesAsync();

                await SyncHarvestStatusAsync(entity.HarvestId);

                return new ApiResponse<HarvestDetailResponse>
                {
                    Success = true,
                    Message = "Ghi nhận thu hoạch thành công",
                    Data = HarvestDetailMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestDetailResponse> { Success = false, Message = "Lỗi ghi nhận thu hoạch", Errors = new List<string> { ex.Message } };
            }
        }

        private async System.Threading.Tasks.Task SyncHarvestStatusAsync(Guid harvestId)
        {
            var harvest = await _harvestRepo.GetByIdAsync(harvestId);
            if (harvest == null) return;
            if (harvest.Status == "cancelled" || harvest.Status == "completed") return;

            var details = harvest.HarvestDetails?.ToList() ?? new List<HarvestDetail>();
            if (details.Count == 0) return;

            var allHarvested = details.All(d => d.ActualHarvestDate.HasValue);
            var anyHarvested = details.Any(d => d.ActualHarvestDate.HasValue);

            string? newStatus = null;
            if (allHarvested) newStatus = "completed";
            else if (anyHarvested && harvest.Status == "planned") newStatus = "harvesting";

            if (newStatus == null) return;

            harvest.Status = newStatus;
            harvest.UpdatedAt = DateTime.UtcNow;
            _harvestRepo.Update(harvest);
            await _harvestRepo.SaveChangesAsync();
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy harvest detail" };

                _repo.Delete(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Đã xóa harvest detail" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi xóa", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
