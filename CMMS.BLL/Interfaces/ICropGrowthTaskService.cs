using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ICropGrowthTaskService
    {
        Task<ApiResponse<IEnumerable<CropGrowthTaskResponse>>> GetAllAsync();
        Task<ApiResponse<CropGrowthTaskResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<CropGrowthTaskResponse>>> GetByStageIdAsync(Guid stageId);
        Task<ApiResponse<string>> CreateAsync(CropGrowthTaskRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, CropGrowthTaskRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
        Task<bool> SaveAsync();
    }
}
