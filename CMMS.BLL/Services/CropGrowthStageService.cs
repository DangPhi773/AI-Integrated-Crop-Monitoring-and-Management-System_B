using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class CropGrowthStageService : ICropGrowthStageService
    {
        private readonly ICropGrowthStageRepository _repo;
        public CropGrowthStageService(ICropGrowthStageRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<CropGrowthStageResponse>>> GetStagesAsync()
        {
            var stages = await _repo.GetAllAsync();
            var data = stages.Select(s => new CropGrowthStageResponse
            {
                StageId = s.StageId,
                CropId = s.CropId,
                CropName = s.Crop?.CropName,
                StageName = s.StageName,
                TemperatureMin = s.TemperatureMin,
                CreatedAt = s.CreatedAt
            });
            return new ApiResponse<IEnumerable<CropGrowthStageResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<string>> CreateStageAsync(CropGrowthStageRequest request)
        {
            var stage = new CropGrowthStage
            {
                StageId = Guid.NewGuid(),
                CropId = request.CropId,
                StageName = request.StageName,
                StageDescription = request.StageDescription,
                TemperatureMin = request.TemperatureMin,
                HumidityMin = request.HumidityMin,
                SoilMoistureMin = request.SoilMoistureMin,
                GrowthIndicators = request.GrowthIndicators,
                CommonDiseases = request.CommonDiseases,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(stage);
            return await _repo.SaveChangesAsync() ?
                new ApiResponse<string> { Success = true, Message = "Tạo giai đoạn thành công" } :
                new ApiResponse<string> { Success = false, Message = "Lỗi lưu dữ liệu" };
        }

        public async Task<ApiResponse<string>> UpdateStageAsync(Guid id, CropGrowthStageRequest request)
        {
            var stage = await _repo.GetByIdAsync(id);
            if (stage == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

            stage.StageName = request.StageName;
            stage.StageDescription = request.StageDescription;
            stage.TemperatureMin = request.TemperatureMin;
            stage.UpdatedAt = DateTime.UtcNow;

            _repo.Update(stage);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
        }

        public async Task<ApiResponse<string>> RemoveStageAsync(Guid id)
        {
            var stage = await _repo.GetByIdAsync(id);
            if (stage == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };
            _repo.Delete(stage);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
        }

        public async Task<ApiResponse<CropGrowthStageResponse>> GetStageByIdAsync(Guid id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return new ApiResponse<CropGrowthStageResponse> { Success = false, Message = "Không tìm thấy" };
            return new ApiResponse<CropGrowthStageResponse> { Success = true, Data = new CropGrowthStageResponse { StageId = s.StageId, StageName = s.StageName } };
        }
    }
}
