using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.SeasonsDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ISeasonsDetailService
    {
        Task<ApiResponse<IEnumerable<SeasonsDetailResponse>>> GetAllSeasonsDetailsAsync();
        Task<ApiResponse<SeasonsDetailResponse>> GetSeasonsDetailByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateSeasonsDetailAsync(SeasonsDetailRequest request);
        Task<ApiResponse<string>> UpdateSeasonsDetailAsync(Guid id, SeasonsDetailRequest request);
        Task<ApiResponse<string>> DeleteSeasonsDetailAsync(Guid id);
    }
}
