using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;

namespace CMMS.BLL.Interfaces
{
    public interface IHarvestService
    {
        Task<ApiResponse<IEnumerable<HarvestSummary>>> GetAllAsync();
        Task<ApiResponse<HarvestResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<HarvestSummary>>> GetByPlotIdAsync(Guid plotId);
        Task<ApiResponse<IEnumerable<HarvestSummary>>> GetBySeasonIdAsync(Guid seasonId);
        Task<ApiResponse<HarvestResponse>> CreateAsync(CreateHarvestRequest request);
        Task<ApiResponse<HarvestResponse>> UpdateAsync(Guid id, UpdateHarvestRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
