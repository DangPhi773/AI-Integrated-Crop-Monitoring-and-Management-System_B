using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;

namespace CMMS.BLL.Interfaces
{
    public interface IHarvestRecordService
    {
        Task<ApiResponse<HarvestRecordResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<HarvestRecordResponse>>> GetByHarvestIdAsync(Guid harvestId);
        Task<ApiResponse<HarvestRecordResponse>> CreateAsync(CreateHarvestRecordRequest request);
        Task<ApiResponse<HarvestRecordResponse>> UpdateAsync(Guid id, UpdateHarvestRecordRequest request);
        Task<ApiResponse<HarvestRecordResponse>> RecordSaleAsync(Guid id, RecordSaleRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
