using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Plots;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class PlotService : IPlotService
    {
        private readonly IPlotRepository _plotRepo;

        public PlotService(IPlotRepository plotRepo) => _plotRepo = plotRepo;

        public async Task<ApiResponse<IEnumerable<PlotResponse>>> GetAllPlotsAsync()
        {
            try
            {
                var plots = await _plotRepo.GetAllAsync();
                var data = plots.Select(MapToResponse);
                return new ApiResponse<IEnumerable<PlotResponse>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PlotResponse>> { Success = false, Message = "Error fetching plots", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<PlotResponse>> GetPlotByIdAsync(Guid id)
        {
            try
            {
                var plot = await _plotRepo.GetByIdAsync(id);
                if (plot == null) return new ApiResponse<PlotResponse> { Success = false, Message = "Plot not found" };
                return new ApiResponse<PlotResponse> { Success = true, Data = MapToResponse(plot) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PlotResponse> { Success = false, Message = "Error fetching plot", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> CreatePlotAsync(PlotRequest request)
        {
            try
            {
                var entity = new Plot
                {
                    PlotId = Guid.NewGuid(),
                    FarmId = request.FarmId,
                    SoilId = request.SoilId,
                    PlotName = request.PlotName,
                    PlotArea = request.PlotArea,
                    PlotStatus = request.PlotStatus ?? "Active",
                    BedCreatedAt = DateTime.UtcNow
                };

                await _plotRepo.AddAsync(entity);
                if (await _plotRepo.SaveChangesAsync())
                    return new ApiResponse<string> { Success = true, Message = "Plot created" };

                return new ApiResponse<string> { Success = false, Message = "Failed to save plot" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error creating plot", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> UpdatePlotAsync(Guid id, PlotRequest request)
        {
            try
            {
                var entity = await _plotRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Plot not found" };

                entity.FarmId = request.FarmId ?? entity.FarmId;
                entity.SoilId = request.SoilId ?? entity.SoilId;
                entity.PlotName = request.PlotName ?? entity.PlotName;
                entity.PlotArea = request.PlotArea ?? entity.PlotArea;
                entity.PlotStatus = request.PlotStatus ?? entity.PlotStatus;

                _plotRepo.Update(entity);
                await _plotRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Plot updated" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error updating plot", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> DeletePlotAsync(Guid id)
        {
            try
            {
                var entity = await _plotRepo.GetByIdAsync(id);
                if (entity == null) return new ApiResponse<string> { Success = false, Message = "Plot not found" };

                _plotRepo.Delete(entity);
                await _plotRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Plot deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Error deleting plot", Errors = new List<string> { ex.Message } };
            }
        }

        private static PlotResponse MapToResponse(Plot p) =>
            new PlotResponse
            {
                PlotId = p.PlotId,
                FarmId = p.FarmId,
                SoilId = p.SoilId,
                PlotName = p.PlotName,
                PlotArea = p.PlotArea,
                PlotStatus = p.PlotStatus,
                BedCreatedAt = p.BedCreatedAt,
                FarmName = p.Farm?.FarmName,
                SoilName = p.Soil?.Name,
                BedsCount = p.Beds?.Count ?? 0
            };
    }
}
