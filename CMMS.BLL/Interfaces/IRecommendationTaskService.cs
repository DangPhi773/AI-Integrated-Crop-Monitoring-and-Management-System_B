using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IRecommendationTaskService
    {
        Task<ApiResponse<IEnumerable<RecommendationTaskResponse>>> GetAllAsync();
        Task<ApiResponse<RecommendationTaskResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(RecommendationTaskRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationTaskRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
