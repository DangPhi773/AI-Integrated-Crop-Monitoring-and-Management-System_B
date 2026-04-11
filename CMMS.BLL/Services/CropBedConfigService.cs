using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.CropBedConfigs;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class CropBedConfigService : ICropBedConfigService
    {
        private readonly ICropBedConfigRepository _repo;
        public CropBedConfigService(ICropBedConfigRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<CropBedConfigResponse>>> GetAllAsync()
        {
            try
            {
                var data = (await _repo.GetAllAsync()).Select(CropBedConfigMapper.ToResponse);
                return new ApiResponse<IEnumerable<CropBedConfigResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CropBedConfigResponse>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<CropBedConfigResponse>> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return new ApiResponse<CropBedConfigResponse> { Success = false, Message = "Không tìm thấy config" };
            return new ApiResponse<CropBedConfigResponse> { Success = true, Data = CropBedConfigMapper.ToResponse(entity) };
        }

        public async Task<ApiResponse<IEnumerable<CropBedConfigResponse>>> GetByCropIdAsync(Guid cropId)
        {
            var data = (await _repo.GetByCropIdAsync(cropId)).Select(CropBedConfigMapper.ToResponse);
            return new ApiResponse<IEnumerable<CropBedConfigResponse>> { Success = true, Data = data };
        }

        public async Task<ApiResponse<string>> CreateAsync(CropBedConfigRequest request)
        {
            try
            {
                if (request.RowsPerBed <= 0 || request.RowSpacing <= 0 || request.PlantSpacing <= 0)
                    return new ApiResponse<string> { Success = false, Message = "rows_per_bed/row_spacing/plant_spacing phải > 0" };

                var pattern = (request.PlantingPattern ?? "straight").ToLowerInvariant();
                if (pattern != "straight" && pattern != "staggered")
                    return new ApiResponse<string> { Success = false, Message = "planting_pattern chỉ chấp nhận 'straight' hoặc 'staggered'" };

                var existed = await _repo.GetByCropAndPatternAsync(request.CropId, pattern);
                if (existed != null)
                    return new ApiResponse<string> { Success = false, Message = "Đã tồn tại config cho crop + pattern này" };

                var entity = new CropBedConfig
                {
                    ConfigId = Guid.NewGuid(),
                    CropId = request.CropId,
                    PlantingPattern = pattern,
                    RowSpacing = request.RowSpacing,
                    PlantSpacing = request.PlantSpacing,
                    RowsPerBed = request.RowsPerBed,
                    BedWidthMin = request.BedWidthMin,
                    BedWidthMax = request.BedWidthMax,
                    PathWidthMin = request.PathWidthMin,
                    PathWidthMax = request.PathWidthMax,
                    BedHeight = request.BedHeight,
                    DensityPerHaMin = request.DensityPerHaMin,
                    DensityPerHaMax = request.DensityPerHaMax,
                    IsDefault = request.IsDefault,
                    Notes = request.Notes,
                    CreatedAt = DateTimeHelper.VnNow(),
                    UpdatedAt = DateTimeHelper.VnNow()
                };

                await _repo.AddAsync(entity);
                return await _repo.SaveChangesAsync()
                    ? new ApiResponse<string> { Success = true, Message = "Tạo config thành công" }
                    : new ApiResponse<string> { Success = false, Message = "Lưu thất bại" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, CropBedConfigRequest request)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };

                entity.RowSpacing = request.RowSpacing;
                entity.PlantSpacing = request.PlantSpacing;
                entity.RowsPerBed = request.RowsPerBed;
                entity.BedWidthMin = request.BedWidthMin ?? entity.BedWidthMin;
                entity.BedWidthMax = request.BedWidthMax ?? entity.BedWidthMax;
                entity.PathWidthMin = request.PathWidthMin ?? entity.PathWidthMin;
                entity.PathWidthMax = request.PathWidthMax ?? entity.PathWidthMax;
                entity.BedHeight = request.BedHeight ?? entity.BedHeight;
                entity.DensityPerHaMin = request.DensityPerHaMin ?? entity.DensityPerHaMin;
                entity.DensityPerHaMax = request.DensityPerHaMax ?? entity.DensityPerHaMax;
                entity.IsDefault = request.IsDefault;
                entity.Notes = request.Notes ?? entity.Notes;
                entity.UpdatedAt = DateTimeHelper.VnNow();

                _repo.Update(entity);
                await _repo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy" };
            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công" };
        }

    }
}
