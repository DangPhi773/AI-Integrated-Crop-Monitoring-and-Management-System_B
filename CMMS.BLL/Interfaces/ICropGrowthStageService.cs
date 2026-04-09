using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ICropGrowthStageService
    {
        Task<ApiResponse<IEnumerable<CropGrowthStageResponse>>> GetStagesAsync();
        Task<ApiResponse<CropGrowthStageResponse>> GetStageByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateStageAsync(CropGrowthStageRequest request);
        Task<ApiResponse<string>> UpdateStageAsync(Guid id, CropGrowthStageRequest request);
        Task<ApiResponse<string>> RemoveStageAsync(Guid id);
    }
}
