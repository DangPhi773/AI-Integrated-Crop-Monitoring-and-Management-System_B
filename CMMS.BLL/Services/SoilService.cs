using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Soils;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class SoilService : ISoilService
    {
        private readonly ISoilRepository _soilRepo;

        public SoilService(ISoilRepository soilRepo) => _soilRepo = soilRepo;

        public async Task<ApiResponse<IEnumerable<SoilResponse>>> GetAllSoilsAsync()
        {
            try
            {
                var soils = await _soilRepo.GetAllAsync();
                var data = soils.Select(MapToResponse);
                return new ApiResponse<IEnumerable<SoilResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SoilResponse>> { Success = false, Message = "Error fetching soils", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<SoilResponse>> GetSoilByIdAsync(Guid id)
        {
            try
            {
                var soil = await _soilRepo.GetByIdAsync(id);
                if (soil == null) return new ApiResponse<SoilResponse> { Success = false, Message = "Soil not found" };
                return new ApiResponse<SoilResponse> { Success = true, Data = MapToResponse(soil) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SoilResponse> { Success = false, Message = "Error fetching soil", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateSoilAsync(SoilRequest request)
        {
            try
            {
                var entity = new Soil
                {
                    SoilId = Guid.NewGuid(),
                    Name = request.Name,
                    ScienceName = request.ScienceName
                };

                await _soilRepo.AddAsync(entity);
                if (await _soilRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Soil created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save soil" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating soil", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateSoilAsync(Guid id, SoilRequest request)
        {
            try
            {
                var entity = await _soilRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Soil not found" };

                entity.Name = request.Name ?? entity.Name;
                entity.ScienceName = request.ScienceName ?? entity.ScienceName;

                _soilRepo.Update(entity);
                await _soilRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Soil updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating soil", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteSoilAsync(Guid id)
        {
            try
            {
                var entity = await _soilRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Soil not found" };

                _soilRepo.Delete(entity);
                await _soilRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Soil deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting soil", Errors = new List<string> { ex.Message } };
            }
        }

        private static SoilResponse MapToResponse(Soil s) =>
            new SoilResponse
            {
                SoilId = s.SoilId,
                Name = s.Name,
                ScienceName = s.ScienceName,
                CropsCount = s.Crops?.Count ?? 0,
                PlotsCount = s.Plots?.Count ?? 0
            };
    }
}
