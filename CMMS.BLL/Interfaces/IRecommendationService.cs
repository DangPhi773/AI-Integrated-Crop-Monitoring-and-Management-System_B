using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Recommendation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IRecommendationService
    {
        Task<ApiResponse<IEnumerable<RecommendationResponse>>> GetListAsync();
        Task<ApiResponse<RecommendationResponse>> GetDetailAsync(Guid id);
        Task<ApiResponse<IEnumerable<RecommendationResponse>>> GetByDiagnosisIdAsync(Guid diagnosisId);
        Task<ApiResponse<string>> CreateAsync(RecommendationRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, RecommendationRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
