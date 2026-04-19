using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Farms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IFarmService
    {
        Task<ApiResponse<IEnumerable<FarmResponse>>> GetAllFarmsAsync();
        Task<ApiResponse<FarmResponse>> GetFarmByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateFarmAsync(FarmRequest request);
        Task<ApiResponse<string>> UpdateFarmAsync(Guid id, FarmRequest request);
        Task<ApiResponse<string>> DeleteFarmAsync(Guid id);
    }
}
