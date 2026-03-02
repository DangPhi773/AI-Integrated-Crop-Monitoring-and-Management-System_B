using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ISeasonService
    {
        System.Threading.Tasks.Task<ApiResponse<IEnumerable<SeasonResponse>>> GetAllSeasonsAsync();
        System.Threading.Tasks.Task<ApiResponse<SeasonResponse>> GetSeasonByIdAsync(Guid id);
        System.Threading.Tasks.Task<ApiResponse<string>> CreateSeasonAsync(SeasonRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> UpdateSeasonAsync(Guid id, SeasonRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> DeleteSeasonAsync(Guid id);
    }
}
