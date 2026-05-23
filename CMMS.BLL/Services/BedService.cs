using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class BedService : IBedService
    {
        private readonly IBedRepository _bedRepo;
        private readonly IPlotRepository _plotRepo;
        private readonly ICropRepository _cropRepo;

        public BedService(
            IBedRepository bedRepo,
            IPlotRepository plotRepo,
            ICropRepository cropRepo)
        {
            _bedRepo = bedRepo;
            _plotRepo = plotRepo;
            _cropRepo = cropRepo;
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
                    BedWidth = request.BedWidth,
                    BedLength = request.BedLength,
                    PathWidth = request.PathWidth,
                    PlantCount = request.PlantCount,
                    RowCount = request.RowCount,
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
                entity.BedWidth = request.BedWidth ?? entity.BedWidth;
                entity.BedLength = request.BedLength ?? entity.BedLength;
                entity.PathWidth = request.PathWidth ?? entity.PathWidth;
                entity.PlantCount = request.PlantCount ?? entity.PlantCount;
                entity.RowCount = request.RowCount ?? entity.RowCount;

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

                if (entity.BedStatus != null && entity.BedStatus.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Luống này đang trong quá trình canh tác (Active). Phải kết thúc mùa vụ hoặc giải phóng luống trước khi xóa!"
                    };
                }

                if (entity.HarvestDetails != null && entity.HarvestDetails.Any())
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Luống này đã có dữ liệu lịch sử canh tác. Để bảo toàn dữ liệu, Nên đổi trạng thái sang 'Empty' thay vì xóa vĩnh viễn."
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

        public async Task<ApiResponse<BedSplitPreview>> PreviewAutoAllocateAsync(BedSplitRequest request)
        {
            try
            {
                var (preview, error) = await BuildSplitAsync(request);
                if (error != null)
                    return new ApiResponse<BedSplitPreview> { Success = false, Message = error };
                return new ApiResponse<BedSplitPreview> { Success = true, Data = preview };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BedSplitPreview> { Success = false, Message = "Lỗi preview", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<string>> ConfirmAutoAllocateAsync(BedSplitConfirmRequest request)
        {
            try
            {
                var plot = await _plotRepo.GetByIdAsync(request.PlotId);
                if (plot == null)
                    return new ApiResponse<string> { Success = false, Message = "Plot không tồn tại" };
                if (plot.Beds != null && plot.Beds.Any())
                    return new ApiResponse<string> { Success = false, Message = "Plot đã có beds" };
                if (request.Beds == null || request.Beds.Count == 0)
                    return new ApiResponse<string> { Success = false, Message = "Danh sách beds rỗng" };

                var now = DateTimeHelper.VnNow();
                foreach (var b in request.Beds)
                {
                    await _bedRepo.AddAsync(new Bed
                    {
                        BedId = Guid.NewGuid(),
                        PlotId = request.PlotId,
                        BedName = b.BedName,
                        BedArea = (decimal)b.BedArea,
                        BedLength = b.BedLength,
                        BedWidth = b.BedWidth,
                        PathWidth = b.PathWidth,
                        RowCount = b.RowCount,
                        PlantCount = b.PlantCount,
                        CropQuantities = b.PlantCount,
                        BedStatus = "Active",
                        BedCreatedAt = now
                    });
                }

                return await _bedRepo.SaveChangesAsync()
                    ? new ApiResponse<string> { Success = true, Message = $"Đã tạo {request.Beds.Count} luống" }
                    : new ApiResponse<string> { Success = false, Message = "Lưu thất bại" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi confirm", Errors = new List<string> { ex.Message } };
            }
        }

        private async Task<(BedSplitPreview? preview, string? error)> BuildSplitAsync(BedSplitRequest request)
        {
            var plot = await _plotRepo.GetByIdAsync(request.PlotId);
            if (plot == null) return (null, "Plot không tồn tại");
            if (!plot.PlotLength.HasValue || plot.PlotLength <= 0) return (null, "Plot thiếu plot_length");
            if (!plot.PlotWidth.HasValue || plot.PlotWidth <= 0) return (null, "Plot thiếu plot_width");

            var crop = await _cropRepo.GetByIdAsync(request.CropId);
            if (crop == null) return (null, "Crop không tồn tại");
            if (!crop.PlantSpacing.HasValue || crop.PlantSpacing <= 0) return (null, "Crop thiếu plant_spacing");
            if (!crop.RowSpacing.HasValue || crop.RowSpacing <= 0) return (null, "Crop thiếu row_spacing");

            if (request.BedWidth <= 0 || request.PathWidth <= 0 || request.RowsPerBed <= 0)
                return (null, "BedWidth/PathWidth/RowsPerBed phải > 0");

            if (request.BedWidth < (request.RowsPerBed - 1) * crop.RowSpacing.Value)
                return (null, "Chiều rộng luống không đủ chứa số hàng yêu cầu");

            double bedLength = plot.PlotLength.Value - 2 * plot.PlotMarginLength;
            if (bedLength <= 0) return (null, "plot_margin_length quá lớn so với plot_length");

            double usableWidth = plot.PlotWidth.Value - 2 * plot.PlotMarginWidth;
            if (usableWidth <= 0) return (null, "plot_margin_width quá lớn so với plot_width");

            int bedCount = (int)Math.Floor(usableWidth / (request.BedWidth + request.PathWidth));
            if (bedCount <= 0) return (null, "Không thể chia luống với thông số hiện tại");

            int plantsPerRow = (int)Math.Floor(bedLength / crop.PlantSpacing.Value);
            int plantCount = plantsPerRow * request.RowsPerBed;
            double bedArea = bedLength * request.BedWidth;
            double widthRemain = usableWidth - bedCount * (request.BedWidth + request.PathWidth);

            var prefix = string.IsNullOrWhiteSpace(request.BedNamePrefix) ? "Luống" : request.BedNamePrefix!.Trim();
            var beds = Enumerable.Range(1, bedCount).Select(i => new BedPreviewItem
            {
                BedName = $"{prefix} {i}",
                BedLength = Math.Round(bedLength, 2),
                BedWidth = Math.Round(request.BedWidth, 2),
                BedArea = Math.Round(bedArea, 2),
                PathWidth = Math.Round(request.PathWidth, 2),
                PlantCount = plantCount,
                RowCount = request.RowsPerBed,
                CropId = request.CropId
            }).ToList();

            return (new BedSplitPreview
            {
                PlotId = request.PlotId,
                CropId = request.CropId,
                BedCount = bedCount,
                BedLength = Math.Round(bedLength, 2),
                BedWidth = Math.Round(request.BedWidth, 2),
                BedArea = Math.Round(bedArea, 2),
                PlantCount = plantCount,
                WidthRemain = Math.Round(widthRemain, 2),
                Beds = beds
            }, null);
        }

        public async Task<ApiResponse<IEnumerable<BedResponse>>> GetBedsByPlotIdAsync(Guid plotId)
        {
            try
            {
                var beds = await _bedRepo.GetBedsByPlotIdAsync(plotId);

                var data = beds.Select(BedMapper.ToResponse);

                return new ApiResponse<IEnumerable<BedResponse>>
                {
                    Success = true,
                    Data = data,
                    Message = data.Any() ? "Lấy danh sách luống thành công" : "Khu vực này chưa có luống nào."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<BedResponse>>
                {
                    Success = false,
                    Message = "Lỗi hệ thống khi lấy danh sách luống",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
