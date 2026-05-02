using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class HarvestDetailService : IHarvestDetailService
    {
        private readonly IHarvestDetailRepository _repo;

        public HarvestDetailService(IHarvestDetailRepository repo) => _repo = repo;

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
