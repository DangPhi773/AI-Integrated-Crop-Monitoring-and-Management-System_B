using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.DTOs.Tasks.CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IRecommendationTaskDetailService
    {
        Task<ApiResponse<IEnumerable<RecommendationTaskDetailResponse>>> GetAllAsync();
        Task<ApiResponse<IEnumerable<RecommendationTaskDetailResponse>>> GetByTaskIdAsync(Guid taskId);
        Task<ApiResponse<RecommendationTaskDetailResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(RecommendationTaskDetailRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationTaskDetailRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}