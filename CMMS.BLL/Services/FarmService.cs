using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Farms;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepo;

        public FarmService(IFarmRepository farmRepo) => _farmRepo = farmRepo;

        public async Task<ApiResponse<IEnumerable<FarmResponse>>> GetAllFarmsAsync()
        {
            try
            {
                var farms = await _farmRepo.GetAllAsync();
                var data = farms.Select(FarmMapper.ToResponse);
                return new ApiResponse<IEnumerable<FarmResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<FarmResponse>> { Success = false, Message = "Error fetching farms", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<FarmResponse>> GetFarmByIdAsync(Guid id)
        {
            try
            {
                var farm = await _farmRepo.GetByIdAsync(id);
                if (farm == null) return new ApiResponse<FarmResponse> { Success = false, Message = "Farm not found" };
                return new ApiResponse<FarmResponse> { Success = true, Data = FarmMapper.ToResponse(farm) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<FarmResponse> { Success = false, Message = "Error fetching farm", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateFarmAsync(FarmRequest request)
        {
            try
            {
                var entity = new Farm
                {
                    FarmId = Guid.NewGuid(),
                    FarmName = request.FarmName,
                    FarmLocation = request.FarmLocation,
                    FarmArea = request.FarmArea,
                    FarmStatus = request.FarmStatus ?? "Active",
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    FarmCreatedAt = DateTimeHelper.VnNow()
                };

                await _farmRepo.AddAsync(entity);
                if (await _farmRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Farm created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save farm" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating farm", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateFarmAsync(Guid id, FarmRequest request)
        {
            try
            {
                var entity = await _farmRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Farm not found" };

                entity.FarmName = request.FarmName ?? entity.FarmName;
                entity.FarmLocation = request.FarmLocation ?? entity.FarmLocation;
                entity.Latitude = request.Latitude ?? entity.Latitude;
                entity.Longitude = request.Longitude ?? entity.Longitude;
                entity.FarmArea = request.FarmArea ?? entity.FarmArea;
                entity.FarmStatus = request.FarmStatus ?? entity.FarmStatus;

                _farmRepo.Update(entity);
                await _farmRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Farm updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating farm", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteFarmAsync(Guid id)
        {
            try
            {
                var entity = await _farmRepo.GetByIdAsync(id);

                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Farm not found" };

                if (entity.Seasons != null && entity.Seasons.Any())
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Không thể xóa Trang trại này vì đang có các Mùa vụ (Seasons) hoạt động bên trong. Hãy xóa các mùa vụ trước!"
                    };
                }

                _farmRepo.Delete(entity);
                await _farmRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Farm deleted successfully" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Lỗi hệ thống khi xóa trang trại",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
