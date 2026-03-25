using CMMS.BLL.Interfaces;
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

                var response = crops.Select(c => new CropResponse
                {
                    CropId = c.CropId,
                    CropName = c.CropName,
                    CropScientificName = c.CropScientificName,
                    CropDefaultGrowthDays = c.CropDefaultGrowthDays,
                    PlantSpacing = c.PlantSpacing,
                    CropQuantities = c.CropQuantities,
                    CropStatus = c.CropStatus,
                    CompatibleSoils = c.SoilCropCompatibilities?.Select(sc => new SoilCompatibilityDto
                    {
                        SoilId = sc.SoilId,
                        SoilName = sc.Soil?.Name,
                        Compatibility = sc.Compatibility
                    }).ToList() ?? new List<SoilCompatibilityDto>()
                }).ToList();

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

                var response = new CropResponse
                {
                    CropId = crop.CropId,
                    CropName = crop.CropName,
                    CropScientificName = crop.CropScientificName,
                    CropDefaultGrowthDays = crop.CropDefaultGrowthDays,
                    PlantSpacing = crop.PlantSpacing,
                    CropQuantities = crop.CropQuantities,
                    CropStatus = crop.CropStatus,
                    CompatibleSoils = crop.SoilCropCompatibilities?.Select(sc => new SoilCompatibilityDto
                    {
                        SoilId = sc.SoilId,
                        SoilName = sc.Soil?.Name,
                        Compatibility = sc.Compatibility
                    }).ToList() ?? new List<SoilCompatibilityDto>()
                };

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

                crop.CropName = request.CropName;
                crop.CropScientificName = request.CropScientificName;
                crop.CropDefaultGrowthDays = request.CropDefaultGrowthDays;
                crop.PlantSpacing = request.PlantSpacing;
                crop.CropQuantities = request.CropQuantities;
                crop.CropStatus = request.CropStatus;

                _cropRepo.Update(crop);
                await _cropRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Errors = new List<string> { ex.Message } };
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
                return new ApiResponse<string> { Success = false, Errors = new List<string> { ex.Message } };
            }
        }
    }
}
