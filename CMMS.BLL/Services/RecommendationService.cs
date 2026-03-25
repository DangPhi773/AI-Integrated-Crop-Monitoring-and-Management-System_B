using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Recommendation;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _repo;
        //private readonly IPestDetectionRepository _pestRepo; 

        public RecommendationService(IRecommendationRepository repo/* IPestDetectionRepository pestRepo */)
        {
            _repo = repo;
            //_pestRepo = pestRepo;
        }

        public async Task<ApiResponse<IEnumerable<RecommendationResponse>>> GetListAsync()
        {
            try
            {
                var data = await _repo.GetAllWithDetailsAsync();
                var response = data.Select(r => new RecommendationResponse
                {
                    RecommendationId = r.RecommendationId,
                    SeasonId = r.SeasonId,
                    PestDetectionId = r.PestDetectionId,
                    Title = r.Title,
                    Content = r.Content,
                    CreatedAt = r.CreatedAt,
                    PestLabel = r.PestDetection?.GeneralLabel,
                    PestSeverity = r.PestDetection?.GeneralSeverity
                }).ToList();

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

            var res = new RecommendationResponse
            {
                RecommendationId = r.RecommendationId,
                SeasonId = r.SeasonId,
                PestDetectionId = r.PestDetectionId,
                Title = r.Title,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                PestLabel = r.PestDetection?.GeneralLabel,
                PestSeverity = r.PestDetection?.GeneralSeverity
            };
            return new ApiResponse<RecommendationResponse> { Success = true, Data = res };
        }

        public async Task<ApiResponse<string>> CreateAsync(RecommendationRequest request)
        {
            try
            {
                var rec = new Recommendation
                {
                    RecommendationId = Guid.NewGuid(),
                    SeasonId = request.SeasonId,
                    PestDetectionId = request.PestDetectionId,
                    Title = request.Title,
                    Content = request.Content,
                    CreatedAt = DateTimeHelper.VnNow(),
                    UpdatedAt = DateTimeHelper.VnNow()
                };

                await _repo.AddAsync(rec);

                // LOGIC THÊM: Cập nhật trạng thái bệnh đã được tư vấn
                //if (request.PestDetectionId.HasValue)
                //{
                //    var pest = await _pestRepo.GetByIdAsync(request.PestDetectionId.Value);
                //    if (pest != null)
                //    {
                //        pest.DetectionStatus = "Recommended"; // Đánh dấu đã có khuyến nghị
                //        _pestRepo.Update(pest);
                //    }
                //}

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
            //var rec = await _pestRepo.GetByIdAsync(id); 
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
