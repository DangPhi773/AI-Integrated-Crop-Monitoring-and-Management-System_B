using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
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
        private readonly IPlotRepository _plotRepo;
        private readonly ICropRepository _cropRepo;
        private readonly ICropBedConfigRepository _configRepo;

        public BedService(
            IBedRepository bedRepo,
            IPlotRepository plotRepo,
            ICropRepository cropRepo,
            ICropBedConfigRepository configRepo)
        {
            _bedRepo = bedRepo;
            _plotRepo = plotRepo;
            _cropRepo = cropRepo;
            _configRepo = configRepo;
        }

        public async Task<ApiResponse<IEnumerable<BedResponse>>> GetAllBedsAsync()
        {
            try
            {
                var beds = await _bedRepo.GetAllAsync();
                var data = beds.Select(BedMapper.ToResponse);
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
                return new ApiResponse<BedResponse> { Success = true, Data = BedMapper.ToResponse(bed) };
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

                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy Luống (Bed) này." };

                if (entity.BedStatus != null && entity.BedStatus.Equals("Occupied", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Luống này đang trong quá trình canh tác (Occupied). Sếp phải kết thúc mùa vụ hoặc giải phóng luống trước khi xóa!"
                    };
                }

                if (entity.SeasonsDetails != null && entity.SeasonsDetails.Any())
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Luống này đã có dữ liệu lịch sử canh tác. Để bảo toàn dữ liệu, sếp nên đổi trạng thái sang 'Inactive' thay vì xóa vĩnh viễn."
                    };
                }

                _bedRepo.Delete(entity);
                await _bedRepo.SaveChangesAsync();

                return new ApiResponse<string> { Success = true, Message = "Xóa Luống thành công." };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Lỗi hệ thống khi xóa luống",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<BedAutoAllocateResponse>> PreviewAutoAllocateAsync(BedAutoAllocateRequest request)
        {
            try
            {
                var calc = await BuildAllocationAsync(request);
                return calc;
            }
            catch (Exception ex)
            {
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Lỗi preview", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<BedAutoAllocateResponse>> ConfirmAutoAllocateAsync(BedAutoAllocateRequest request)
        {
            try
            {
                var calc = await BuildAllocationAsync(request);
                if (!calc.Success || calc.Data == null) return calc;

                var plot = await _plotRepo.GetByIdAsync(request.PlotId);
                if (plot == null)
                    return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot không tồn tại" };
                if (plot.Beds != null && plot.Beds.Any())
                    return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot đã có beds, không thể auto-allocate" };

                var now = DateTimeHelper.VnNow();
                foreach (var b in calc.Data.Beds)
                {
                    var entity = new Bed
                    {
                        BedId = Guid.NewGuid(),
                        PlotId = request.PlotId,
                        BedName = b.BedName,
                        BedArea = (decimal)b.BedArea,
                        BedStatus = "Active",
                        CropQuantities = b.PlantCount,
                        BedCreatedAt = now,
                        PlantingPattern = calc.Data.PlantingPattern,
                        RowCount = b.RowCount,
                        BedWidth = b.BedWidth,
                        BedLength = b.BedLength
                    };
                    await _bedRepo.AddAsync(entity);
                }

                if (await _bedRepo.SaveChangesAsync())
                    return calc;

                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Lưu thất bại" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Lỗi confirm", Errors = new List<string> { ex.Message } };
            }
        }

        private async Task<ApiResponse<BedAutoAllocateResponse>> BuildAllocationAsync(BedAutoAllocateRequest request)
        {
            var plot = await _plotRepo.GetByIdAsync(request.PlotId);
            if (plot == null)
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot không tồn tại" };
            if (plot.PlotArea == null || plot.PlotArea <= 0)
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot chưa có diện tích hợp lệ" };
            if (plot.Beds != null && plot.Beds.Any())
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot đã có beds" };

            var crop = await _cropRepo.GetByIdAsync(request.CropId);
            if (crop == null)
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Crop không tồn tại" };

            var availableConfigs = (await _configRepo.GetByCropIdAsync(request.CropId)).ToList();
            if (!availableConfigs.Any())
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Chưa có cấu hình luống cho giống cây này" };

            CropBedConfig? config;
            var pattern = request.PlantingPattern?.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(pattern))
                config = availableConfigs.FirstOrDefault(c => c.PlantingPattern == pattern);
            else
                config = availableConfigs.FirstOrDefault(c => c.IsDefault) ?? availableConfigs.First();

            if (config == null)
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = $"Không tìm thấy config với pattern '{request.PlantingPattern}'" };

            var warnings = new List<string>();

            double plotAreaM2 = (double)plot.PlotArea!.Value;
            double plotLength, plotWidth;
            bool isEstimated = true;
            if (plot.PlotLength.HasValue && plot.PlotWidth.HasValue && plot.PlotLength > 0 && plot.PlotWidth > 0)
            {
                plotLength = plot.PlotLength.Value;
                plotWidth = plot.PlotWidth.Value;
                isEstimated = false;
            }
            else
            {
                plotLength = Math.Sqrt(plotAreaM2);
                plotWidth = plotLength;
                warnings.Add("Plot không có length/width — ước lượng hình vuông từ plot_area");
            }

            const double margin = 0.15;
            double bedWidth = (config.RowsPerBed - 1) * config.RowSpacing + 2 * margin;
            if (config.BedWidthMin.HasValue && bedWidth < config.BedWidthMin.Value)
                warnings.Add($"bed_width tính ra ({bedWidth:F2}m) nhỏ hơn min ({config.BedWidthMin}m)");
            if (config.BedWidthMax.HasValue && bedWidth > config.BedWidthMax.Value)
                warnings.Add($"bed_width tính ra ({bedWidth:F2}m) lớn hơn max ({config.BedWidthMax}m)");

            double pathWidth = ((config.PathWidthMin ?? 0.2) + (config.PathWidthMax ?? 0.3)) / 2.0;

            double slot = bedWidth + pathWidth;
            int maxBedCount = (int)Math.Floor(plotWidth / slot);
            if (maxBedCount <= 0)
                return new ApiResponse<BedAutoAllocateResponse> { Success = false, Message = "Plot quá nhỏ để chứa 1 luống với cấu hình này" };

            int bedCount = maxBedCount;
            if (request.DesiredBedCount.HasValue && request.DesiredBedCount.Value > 0)
            {
                if (request.DesiredBedCount.Value > maxBedCount)
                    warnings.Add($"desired_bed_count ({request.DesiredBedCount}) vượt tối đa ({maxBedCount}), dùng {maxBedCount}");
                else
                    bedCount = request.DesiredBedCount.Value;
            }

            double bedLength = plotLength;

            int plantsPerBed = CalculatePlants(config, bedLength);

            int totalPlants = plantsPerBed * bedCount;
            double bedAreaSingle = bedWidth * bedLength;
            double usedArea = bedCount * (bedWidth + pathWidth) * bedLength;
            if (usedArea > plotAreaM2) usedArea = plotAreaM2;
            double unusedArea = Math.Max(0, plotAreaM2 - usedArea);

            double densityPerHa = (totalPlants / plotAreaM2) * 10000.0;
            bool densityWarning = false;
            string? densityMsg = null;
            if (config.DensityPerHaMin.HasValue && densityPerHa < config.DensityPerHaMin.Value)
            {
                densityWarning = true;
                densityMsg = $"Mật độ {densityPerHa:F0} cây/ha thấp hơn khuyến nghị ({config.DensityPerHaMin})";
            }
            else if (config.DensityPerHaMax.HasValue && densityPerHa > config.DensityPerHaMax.Value)
            {
                densityWarning = true;
                densityMsg = $"Mật độ {densityPerHa:F0} cây/ha vượt khuyến nghị ({config.DensityPerHaMax})";
            }

            var prefix = string.IsNullOrWhiteSpace(request.BedNamePrefix) ? "Luống" : request.BedNamePrefix!.Trim();
            var beds = Enumerable.Range(1, bedCount).Select(i => new BedAllocationItem
            {
                BedName = $"{prefix} {i}",
                BedWidth = Math.Round(bedWidth, 2),
                BedLength = Math.Round(bedLength, 2),
                BedArea = Math.Round(bedAreaSingle, 2),
                RowCount = config.RowsPerBed,
                PlantCount = plantsPerBed
            }).ToList();

            var response = new BedAutoAllocateResponse
            {
                PlotId = request.PlotId,
                CropId = request.CropId,
                PlantingPattern = config.PlantingPattern,
                AvailablePatterns = availableConfigs.Select(c => c.PlantingPattern).Distinct().ToList(),
                PlotAreaM2 = Math.Round(plotAreaM2, 2),
                EstimatedPlotSideM = Math.Round(Math.Sqrt(plotAreaM2), 2),
                UsedAreaM2 = Math.Round(usedArea, 2),
                UnusedAreaM2 = Math.Round(unusedArea, 2),
                BedCount = bedCount,
                TotalPlantCount = totalPlants,
                DensityPerHa = Math.Round(densityPerHa, 0),
                DensityWarning = densityWarning,
                DensityWarningMessage = densityMsg,
                Warnings = warnings,
                Beds = beds,
                IsEstimatedShape = isEstimated
            };

            return new ApiResponse<BedAutoAllocateResponse> { Success = true, Data = response };
        }

        private static int CalculatePlants(CropBedConfig config, double bedLength)
        {
            if (config.PlantSpacing <= 0) return 0;

            if (config.PlantingPattern == "staggered")
            {
                int oddRowPlants = (int)Math.Floor(bedLength / config.PlantSpacing);
                int evenRowPlants = (int)Math.Floor((bedLength - config.PlantSpacing / 2.0) / config.PlantSpacing);
                if (evenRowPlants < 0) evenRowPlants = 0;

                int oddRows = (config.RowsPerBed + 1) / 2;
                int evenRows = config.RowsPerBed / 2;
                return oddRows * oddRowPlants + evenRows * evenRowPlants;
            }
            else
            {
                int perRow = (int)Math.Floor(bedLength / config.PlantSpacing);
                return config.RowsPerBed * perRow;
            }
        }

    }
}
