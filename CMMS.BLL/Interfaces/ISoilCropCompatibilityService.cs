using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ISoilCropCompatibilityService
    {
        Task<ApiResponse<IEnumerable<SoilCropCompatibilityResponse>>> GetAllAsync();
        Task<ApiResponse<SoilCropCompatibilityResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(SoilCropCompatibilityRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, SoilCropCompatibilityRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
