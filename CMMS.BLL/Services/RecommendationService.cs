using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Recommendation;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _repo;

        public RecommendationService(IRecommendationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<RecommendationResponse>>> GetListAsync()
        {
            try
            {
                var data = await _repo.GetAllWithDetailsAsync();
                var response = data.Select(RecommendationMapper.ToResponse).ToList();

                return new ApiResponse<IEnumerable<RecommendationResponse>> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<RecommendationResponse>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<RecommendationResponse>> GetDetailAsync(Guid id)
        {
            var r = await _repo.GetByIdWithDetailsAsync(id);
            if (r == null) return new ApiResponse<RecommendationResponse> { Success = false, Message = "Không tìm thấy" };

            var res = RecommendationMapper.ToResponse(r);
            return new ApiResponse<RecommendationResponse> { Success = true, Data = res };
        }

        public async Task<ApiResponse<IEnumerable<RecommendationResponse>>> GetByDiagnosisIdAsync(Guid diagnosisId)
        {
            try
            {
                var data = await _repo.GetByDiagnosisIdAsync(diagnosisId);
                var response = data.Select(RecommendationMapper.ToResponse).ToList();
                return new ApiResponse<IEnumerable<RecommendationResponse>> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<RecommendationResponse>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<string>> CreateAsync(RecommendationRequest request)
        {
            try
            {
                var rec = new Recommendation
                {
                    RecommendationId = Guid.NewGuid(),
                    SeasonId = request.SeasonId,
                    DiagnosisId = request.DiagnosisId,
                    Title = request.Title,
                    Content = request.Content,
                    CreatedAt = DateTimeHelper.VnNow(),
                    UpdatedAt = DateTimeHelper.VnNow()
                };

                await _repo.AddAsync(rec);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Gửi khuyến nghị thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi khi tạo: " + ex.Message };
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationRequest request)
        {
            var existing = await _repo.GetByIdWithDetailsAsync(id);
            if (existing == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

            existing.Title = request.Title;
            existing.Content = request.Content;
            existing.UpdatedAt = DateTimeHelper.VnNow();

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var existing = await _repo.GetByIdWithDetailsAsync(id);
            if (existing == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

            _repo.Delete(existing);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
        }
    }
}
