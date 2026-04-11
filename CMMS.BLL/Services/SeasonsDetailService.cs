using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.SeasonsDetails;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class SeasonsDetailService : ISeasonsDetailService
    {
        private readonly ISeasonsDetailRepository _seasonsDetailRepo;

        public SeasonsDetailService(ISeasonsDetailRepository seasonsDetailRepo) => _seasonsDetailRepo = seasonsDetailRepo;

        public async Task<ApiResponse<IEnumerable<SeasonsDetailResponse>>> GetAllSeasonsDetailsAsync()
        {
            try
            {
                var details = await _seasonsDetailRepo.GetAllAsync();
                var data = details.Select(SeasonsDetailMapper.ToResponse);
                return new ApiResponse<IEnumerable<SeasonsDetailResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SeasonsDetailResponse>> { Success = false, Message = "Error fetching seasons details", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<SeasonsDetailResponse>> GetSeasonsDetailByIdAsync(Guid id)
        {
            try
            {
                var detail = await _seasonsDetailRepo.GetByIdAsync(id);
                if (detail == null) return new ApiResponse<SeasonsDetailResponse> { Success = false, Message = "Seasons detail not found" };
                return new ApiResponse<SeasonsDetailResponse> { Success = true, Data = SeasonsDetailMapper.ToResponse(detail) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SeasonsDetailResponse> { Success = false, Message = "Error fetching seasons detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateSeasonsDetailAsync(SeasonsDetailRequest request)
        {
            try
            {
                var entity = new SeasonsDetail
                {
                    SeasonDetailId = Guid.NewGuid(),
                    SeasonId = request.SeasonId,
                    BedId = request.BedId,
                    CropId = request.CropId,
                    CropQuantity = request.CropQuantity,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    SeasonExpectedHarvestDate = request.SeasonExpectedHarvestDate,
                    TotalHarvestYield = request.TotalHarvestYield
                };

                await _seasonsDetailRepo.AddAsync(entity);
                if (await _seasonsDetailRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Seasons detail created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save seasons detail" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating seasons detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateSeasonsDetailAsync(Guid id, SeasonsDetailRequest request)
        {
            try
            {
                var entity = await _seasonsDetailRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Seasons detail not found" };

                entity.SeasonId = request.SeasonId ?? entity.SeasonId;
                entity.BedId = request.BedId ?? entity.BedId;
                entity.CropId = request.CropId ?? entity.CropId;
                entity.CropQuantity = request.CropQuantity ?? entity.CropQuantity;
                entity.StartDate = request.StartDate ?? entity.StartDate;
                entity.EndDate = request.EndDate ?? entity.EndDate;
                entity.SeasonExpectedHarvestDate = request.SeasonExpectedHarvestDate ?? entity.SeasonExpectedHarvestDate;
                entity.TotalHarvestYield = request.TotalHarvestYield ?? entity.TotalHarvestYield;

                _seasonsDetailRepo.Update(entity);
                await _seasonsDetailRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Seasons detail updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating seasons detail", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteSeasonsDetailAsync(Guid id)
        {
            try
            {
                var entity = await _seasonsDetailRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Seasons detail not found" };

                _seasonsDetailRepo.Delete(entity);
                await _seasonsDetailRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Seasons detail deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting seasons detail", Errors = new List<string> { ex.Message } };
            }
        }

    }
}
