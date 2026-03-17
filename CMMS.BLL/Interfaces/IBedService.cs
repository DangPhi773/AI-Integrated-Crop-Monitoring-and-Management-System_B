using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Beds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IBedService
    {
        Task<ApiResponse<IEnumerable<BedResponse>>> GetAllBedsAsync();
        Task<ApiResponse<BedResponse>> GetBedByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateBedAsync(BedRequest request);
        Task<ApiResponse<string>> UpdateBedAsync(Guid id, BedRequest request);
        Task<ApiResponse<string>> DeleteBedAsync(Guid id);
    }
}
