using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class BedService : IBedService
    {
        private readonly IBedRepository _bedRepo;

        public BedService(IBedRepository bedRepo) => _bedRepo = bedRepo;

        public async Task<ApiResponse<IEnumerable<BedResponse>>> GetAllBedsAsync()
        {
            try
            {
                var beds = await _bedRepo.GetAllAsync();
                var data = beds.Select(MapToResponse);
                return new ApiResponse<IEnumerable<BedResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<BedResponse>> { Success = false, Message = "Error fetching beds", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<BedResponse>> GetBedByIdAsync(Guid id)
        {
            try
            {
                var bed = await _bedRepo.GetByIdAsync(id);
                if (bed == null) return new ApiResponse<BedResponse> { Success = false, Message = "Bed not found" };
                return new ApiResponse<BedResponse> { Success = true, Data = MapToResponse(bed) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BedResponse> { Success = false, Message = "Error fetching bed", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreateBedAsync(BedRequest request)
        {
            try
            {
                var entity = new Bed
                {
                    BedId = Guid.NewGuid(),
                    PlotId = request.PlotId,
                    BedName = request.BedName,
                    BedArea = request.BedArea,
                    BedStatus = request.BedStatus ?? "Active",
                    CropQuantities = request.CropQuantities,
                    BedCreatedAt = DateTimeHelper.VnNow()
                };

                await _bedRepo.AddAsync(entity);
                if (await _bedRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Bed created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save bed" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating bed", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdateBedAsync(Guid id, BedRequest request)
        {
            try
            {
                var entity = await _bedRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Bed not found" };

                entity.PlotId = request.PlotId ?? entity.PlotId;
                entity.BedName = request.BedName ?? entity.BedName;
                entity.BedArea = request.BedArea ?? entity.BedArea;
                entity.BedStatus = request.BedStatus ?? entity.BedStatus;
                entity.CropQuantities = request.CropQuantities ?? entity.CropQuantities;

                _bedRepo.Update(entity);
                await _bedRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Bed updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating bed", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeleteBedAsync(Guid id)
        {
            try
            {
                var entity = await _bedRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Bed not found" };

                _bedRepo.Delete(entity);
                await _bedRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Bed deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting bed", Errors = new List<string> { ex.Message } };
            }
        }

        private static BedResponse MapToResponse(Bed b) =>
            new BedResponse
            {
                BedId = b.BedId,
                PlotId = b.PlotId,
                BedName = b.BedName,
                BedArea = b.BedArea,
                BedStatus = b.BedStatus,
                BedCreatedAt = b.BedCreatedAt,
                CropQuantities = b.CropQuantities,
                PlotName = b.Plot?.PlotName,
                SeasonsDetailsCount = b.SeasonsDetails?.Count ?? 0
            };
    }
}
