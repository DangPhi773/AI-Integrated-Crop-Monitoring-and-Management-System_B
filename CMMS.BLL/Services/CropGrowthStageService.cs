using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
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
            var data = stages.Select(CropGrowthStageMapper.ToResponse);
            return new ApiResponse<IEnumerable<CropGrowthStageResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<IEnumerable<CropGrowthStageResponse>>> GetByCropIdAsync(Guid cropId)
        {
            var stages = await _repo.GetByCropIdAsync(cropId);
            var data = CropGrowthStageMapper.ToResponseList(stages);
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
                TemperatureMax = request.TemperatureMax,
                HumidityMax = request.HumidityMax,
                SoilMoistureMax = request.SoilMoistureMax,
                GrowthIndicators = request.GrowthIndicators,
                CommonDiseases = request.CommonDiseases,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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
            stage.TemperatureMax = request.TemperatureMax;
            stage.HumidityMax = request.HumidityMax;
            stage.SoilMoistureMax = request.SoilMoistureMax;
            stage.GrowthIndicators = request.GrowthIndicators;
            stage.CommonDiseases = request.CommonDiseases;
            stage.Notes = request.Notes;
            stage.UpdatedAt = DateTime.UtcNow;

            _repo.Update(stage);
            return await _repo.SaveChangesAsync() ?
                new ApiResponse<string> { Success = true, Message = "Cập nhật giai đoạn thành công" } :
                new ApiResponse<string> { Success = false, Message = "Lỗi khi cập nhật dữ liệu" };
        }

        public async Task<ApiResponse<string>> RemoveStageAsync(Guid id)
        {
            var stage = await _repo.GetByIdAsync(id);
            if (stage == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };
            _repo.Delete(stage);
            return await _repo.SaveChangesAsync() ?
                new ApiResponse<string> { Success = true, Message = "Xóa giai đoạn thành công" } :
                new ApiResponse<string> { Success = false, Message = "Lỗi khi thực hiện xóa dữ liệu" };
        }

        public async Task<ApiResponse<CropGrowthStageResponse>> GetStageByIdAsync(Guid id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return new ApiResponse<CropGrowthStageResponse> { Success = false, Message = "Không tìm thấy" };
            return new ApiResponse<CropGrowthStageResponse> { Success = true, Data = CropGrowthStageMapper.ToResponse(s) };
        }
    }
}
