using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface ICropService
    {
        System.Threading.Tasks.Task<ApiResponse<IEnumerable<CropResponse>>> GetAllCropsAsync();
        System.Threading.Tasks.Task<ApiResponse<CropResponse>> GetCropByIdAsync(Guid id);
        System.Threading.Tasks.Task<ApiResponse<string>> CreateCropAsync(CropRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> UpdateCropAsync(Guid id, CropRequest request);
        System.Threading.Tasks.Task<ApiResponse<string>> DeleteCropAsync(Guid id);
    }
}
