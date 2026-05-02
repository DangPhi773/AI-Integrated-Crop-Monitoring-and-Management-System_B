using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.GrowthTrackings;

namespace CMMS.BLL.Interfaces
{
    public interface IGrowthTrackingService
    {
        Task<ApiResponse<IEnumerable<GrowthTrackingResponse>>> GetAllAsync();
        Task<ApiResponse<GrowthTrackingResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<GrowthTrackingResponse>>> GetByHarvestDetailIdAsync(Guid harvestDetailId);
        Task<ApiResponse<SeasonProgressResponse>> GetSeasonProgressAsync(Guid seasonId);
        Task<ApiResponse<GrowthTrackingResponse>> CreateAsync(GrowthTrackingRequest request, Guid? userId);
        Task<ApiResponse<GrowthTrackingResponse>> UpdateAsync(Guid id, GrowthTrackingUpdateRequest request, Guid? userId);
        Task<ApiResponse<GrowthTrackingResponse>> AdvanceStageAsync(Guid id, AdvanceStageRequest request, Guid? userId);
        Task<ApiResponse<GrowthTrackingResponse>> CompleteAsync(Guid id, Guid? userId);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
