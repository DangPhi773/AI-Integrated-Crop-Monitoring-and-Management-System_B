using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Seasons;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepo;
        private readonly IFarmRepository _farmRepo;

        public SeasonService(ISeasonRepository seasonRepo, IFarmRepository farmRepo)
        {
            _seasonRepo = seasonRepo;
            _farmRepo = farmRepo;
        }

        public async Task<ApiResponse<IEnumerable<SeasonResponse>>> GetAllSeasonsAsync()
        {
            try
            {
                var seasons = await _seasonRepo.GetAllAsync();
                var data = seasons.Select(SeasonMapper.ToResponse);
                return new ApiResponse<IEnumerable<SeasonResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SeasonResponse>> { Success = false, Message = "Error fetching seasons", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<SeasonResponse>> GetSeasonByIdAsync(Guid id)
        {
            try
            {
                var s = await _seasonRepo.GetByIdAsync(id);
                if (s == null) return new ApiResponse<SeasonResponse> { Success = false, Message = "Season not found" };
                return new ApiResponse<SeasonResponse> { Success = true, Data = SeasonMapper.ToResponse(s) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SeasonResponse> { Success = false, Message = "Error fetching season", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateSeasonAsync(SeasonRequest request)
        {
            try
            {
                if (request.SeasonStartDate.HasValue && request.SeasonEndDate.HasValue
                    && request.SeasonStartDate > request.SeasonEndDate)
                    return new ApiResponse<string> { Success = false, Message = "Ngày bắt đầu không được sau ngày kết thúc" };

                if (string.IsNullOrWhiteSpace(request.SeasonName))
                    return new ApiResponse<string> { Success = false, Message = "Tên mùa vụ là bắt buộc" };

                if (!request.FarmId.HasValue)
                    return new ApiResponse<string> { Success = false, Message = "Trang trại là bắt buộc" };

                var farm = await _farmRepo.GetByIdAsync(request.FarmId.Value);
                if (farm == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy trang trại" };

                var combinedName = $"{request.SeasonName.Trim()} ({farm.FarmName.Trim()})";

                if (await _seasonRepo.ExistsByNameAsync(combinedName))
                    return new ApiResponse<string> { Success = false, Message = "Tên mùa vụ đã tồn tại" };

                var entity = new Season
                {
                    SeasonId = Guid.NewGuid(),
                    FarmId = request.FarmId,
                    SeasonName = combinedName,
                    SeasonStartDate = request.SeasonStartDate,
                    SeasonEndDate = request.SeasonEndDate,
                    Description = request.Description,
                    SeasonNotes = request.SeasonNotes,
                    Status = request.Status ?? "Active",
                    SeasonCreatedAt = DateTimeHelper.VnNow()
                };

                await _seasonRepo.AddAsync(entity);
                if (await _seasonRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Season created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save season" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating season", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateSeasonAsync(Guid id, SeasonRequest request)
        {
            try
            {
                var entity = await _seasonRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Season not found" };

                var newFarmId = request.FarmId ?? entity.FarmId;
                if (!newFarmId.HasValue)
                    return new ApiResponse<string> { Success = false, Message = "Trang trại là bắt buộc" };

                var oldFarm = entity.FarmId.HasValue ? await _farmRepo.GetByIdAsync(entity.FarmId.Value) : null;
                var newFarm = newFarmId == entity.FarmId
                    ? oldFarm
                    : await _farmRepo.GetByIdAsync(newFarmId.Value);
                if (newFarm == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy trang trại" };

                var baseName = !string.IsNullOrWhiteSpace(request.SeasonName)
                    ? request.SeasonName
                    : ExtractBaseName(entity.SeasonName, oldFarm?.FarmName);

                if (string.IsNullOrWhiteSpace(baseName))
                    return new ApiResponse<string> { Success = false, Message = "Tên mùa vụ là bắt buộc" };

                var combinedName = $"{baseName.Trim()} ({newFarm.FarmName.Trim()})";

                if (!string.Equals(combinedName, entity.SeasonName, StringComparison.OrdinalIgnoreCase)
                    && await _seasonRepo.ExistsByNameAsync(combinedName, id))
                    return new ApiResponse<string> { Success = false, Message = "Tên mùa vụ đã tồn tại" };

                entity.FarmId = newFarmId;
                entity.SeasonName = combinedName;
                entity.SeasonStartDate = request.SeasonStartDate ?? entity.SeasonStartDate;
                entity.SeasonEndDate = request.SeasonEndDate ?? entity.SeasonEndDate;
                entity.Description = request.Description ?? entity.Description;
                entity.SeasonNotes = request.SeasonNotes ?? entity.SeasonNotes;
                entity.Status = request.Status ?? entity.Status;

                if (entity.SeasonStartDate.HasValue && entity.SeasonEndDate.HasValue
                    && entity.SeasonStartDate > entity.SeasonEndDate)
                    return new ApiResponse<string> { Success = false, Message = "Ngày bắt đầu không được sau ngày kết thúc" };

                _seasonRepo.Update(entity);
                await _seasonRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Season updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating season", Errors = new List<string> { ex.Message } };
            }
        }

        private static string ExtractBaseName(string? combined, string? farmName)
        {
            if (string.IsNullOrWhiteSpace(combined)) return string.Empty;
            if (!string.IsNullOrWhiteSpace(farmName))
            {
                var suffix = $" ({farmName.Trim()})";
                if (combined.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    return combined.Substring(0, combined.Length - suffix.Length).Trim();
            }
            return combined.Trim();
        }

        public async Task<ApiResponse<string>> DeleteSeasonAsync(Guid id)
        {
            try
            {
                var entity = await _seasonRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Season not found" };

                _seasonRepo.Delete(entity);
                await _seasonRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Season deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting season", Errors = new List<string> { ex.Message } };
            }
        }

    }
}
