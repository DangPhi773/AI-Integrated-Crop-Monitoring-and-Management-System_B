using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;

namespace CMMS.BLL.Interfaces
{
    public interface IHarvestDetailService
    {
        Task<ApiResponse<HarvestDetailResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<HarvestDetailResponse>>> GetByHarvestIdAsync(Guid harvestId);
        Task<ApiResponse<HarvestDetailResponse>> UpdateAsync(Guid id, UpdateHarvestDetailRequest request);
        Task<ApiResponse<HarvestDetailResponse>> RecordHarvestAsync(Guid id, RecordHarvestRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
