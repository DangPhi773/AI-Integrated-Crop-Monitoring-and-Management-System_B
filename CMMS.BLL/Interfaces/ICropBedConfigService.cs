using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.CropBedConfigs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ICropBedConfigService
    {
        Task<ApiResponse<IEnumerable<CropBedConfigResponse>>> GetAllAsync();
        Task<ApiResponse<CropBedConfigResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<CropBedConfigResponse>>> GetByCropIdAsync(Guid cropId);
        Task<ApiResponse<string>> CreateAsync(CropBedConfigRequest request);
        Task<ApiResponse<string>> UpdateAsync(Guid id, CropBedConfigRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
