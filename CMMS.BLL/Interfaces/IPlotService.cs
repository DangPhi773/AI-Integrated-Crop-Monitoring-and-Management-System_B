using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Plots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface IPlotService
    {
        Task<ApiResponse<IEnumerable<PlotResponse>>> GetAllPlotsAsync();
        Task<ApiResponse<PlotResponse>> GetPlotByIdAsync(Guid id);
        Task<ApiResponse<string>> CreatePlotAsync(PlotRequest request);
        Task<ApiResponse<string>> UpdatePlotAsync(Guid id, PlotRequest request);
        Task<ApiResponse<string>> DeletePlotAsync(Guid id);
    }
}
