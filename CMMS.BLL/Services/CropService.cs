using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class CropService : ICropService
    {
        private readonly ICropRepository _cropRepo;
        public CropService(ICropRepository cropRepo) => _cropRepo = cropRepo;

        public async System.Threading.Tasks.Task<ApiResponse<IEnumerable<CropResponse>>> GetAllCropsAsync()
        {
            try
            {
                var crops = await _cropRepo.GetAllAsync();

                var response = crops.Select(CropMapper.ToResponse).ToList();

                return new ApiResponse<IEnumerable<CropResponse>> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CropResponse>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<CropResponse>> GetCropByIdAsync(Guid id)
        {
            try
            {
                var crop = await _cropRepo.GetByIdAsync(id);

                if (crop == null)
                    return new ApiResponse<CropResponse> { Success = false, Message = "Không tìm thấy cây trồng" };

                var response = CropMapper.ToResponse(crop);

                return new ApiResponse<CropResponse> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CropResponse> { Success = false, Message = "Lỗi hệ thống", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> CreateCropAsync(CropRequest request)
        {
            try
            {
                var crop = new Crop
                {
                    CropId = Guid.NewGuid(),
                    CropName = request.CropName,
                    CropScientificName = request.CropScientificName,
                    CropDefaultGrowthDays = request.CropDefaultGrowthDays,
                    PlantSpacing = request.PlantSpacing,
                    CropQuantities = request.CropQuantities,
                    CropStatus = request.CropStatus ?? "Active"
                };
                await _cropRepo.AddAsync(crop);
                await _cropRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> UpdateCropAsync(Guid id, CropRequest request)
        {
            try
            {
                var crop = await _cropRepo.GetByIdAsync(id);
                if (crop == null) return new ApiResponse<string> { Success = false, Message = "Không tồn tại" };

                crop.CropName = !string.IsNullOrWhiteSpace(request.CropName) ? request.CropName : crop.CropName;
                crop.CropScientificName = request.CropScientificName ?? crop.CropScientificName;
                crop.CropDefaultGrowthDays = request.CropDefaultGrowthDays ?? crop.CropDefaultGrowthDays;
                crop.PlantSpacing = request.PlantSpacing ?? crop.PlantSpacing;
                crop.CropQuantities = request.CropQuantities ?? crop.CropQuantities;
                crop.CropStatus = request.CropStatus ?? crop.CropStatus;

                _cropRepo.Update(crop);
                await _cropRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi cập nhật cây trồng", Errors = new List<string> { ex.Message } };
            }
        }

        public async System.Threading.Tasks.Task<ApiResponse<string>> DeleteCropAsync(Guid id)
        {
            try
            {
                var crop = await _cropRepo.GetByIdAsync(id);
                if (crop == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };
                _cropRepo.Delete(crop);
                await _cropRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi xóa cây trồng", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
