using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Soils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ISoilService
    {
        Task<ApiResponse<IEnumerable<SoilResponse>>> GetAllSoilsAsync();
        Task<ApiResponse<SoilResponse>> GetSoilByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateSoilAsync(SoilRequest request);
        Task<ApiResponse<string>> UpdateSoilAsync(Guid id, SoilRequest request);
        Task<ApiResponse<string>> DeleteSoilAsync(Guid id);
    }
}
