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

        public SeasonService(ISeasonRepository seasonRepo) => _seasonRepo = seasonRepo;

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
                var entity = new Season
                {
                    SeasonId = Guid.NewGuid(),
                    FarmId = request.FarmId,
                    SeasonName = request.SeasonName,
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

                entity.FarmId = request.FarmId ?? entity.FarmId;
                entity.SeasonName = request.SeasonName ?? entity.SeasonName;
                entity.SeasonStartDate = request.SeasonStartDate ?? entity.SeasonStartDate;
                entity.SeasonEndDate = request.SeasonEndDate ?? entity.SeasonEndDate;
                entity.Description = request.Description ?? entity.Description;
                entity.SeasonNotes = request.SeasonNotes ?? entity.SeasonNotes;
                entity.Status = request.Status ?? entity.Status;

                _seasonRepo.Update(entity);
                await _seasonRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Season updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating season", Errors = new List<string> { ex.Message } };
            }
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
