using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Harvests;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class HarvestService : IHarvestService
    {
        private static readonly HashSet<string> ValidStatuses =
            new(StringComparer.OrdinalIgnoreCase) { "planned", "growing", "harvesting", "completed", "cancelled" };

        private readonly IHarvestRepository _harvestRepo;
        private readonly IHarvestDetailRepository _detailRepo;
        private readonly IPlotRepository _plotRepo;
        private readonly ISeasonRepository _seasonRepo;
        private readonly ICropRepository _cropRepo;
        private readonly IBedRepository _bedRepo;

        public HarvestService(
            IHarvestRepository harvestRepo,
            IHarvestDetailRepository detailRepo,
            IPlotRepository plotRepo,
            ISeasonRepository seasonRepo,
            ICropRepository cropRepo,
            IBedRepository bedRepo)
        {
            _harvestRepo = harvestRepo;
            _detailRepo = detailRepo;
            _plotRepo = plotRepo;
            _seasonRepo = seasonRepo;
            _cropRepo = cropRepo;
            _bedRepo = bedRepo;
        }

        public async Task<ApiResponse<IEnumerable<HarvestSummary>>> GetAllAsync()
        {
            try
            {
                var items = await _harvestRepo.GetAllAsync();
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = true, Data = items.Select(HarvestMapper.ToSummary) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = false, Message = "Lỗi lấy danh sách harvest", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _harvestRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Không tìm thấy harvest" };
                return new ApiResponse<HarvestResponse> { Success = true, Data = HarvestMapper.ToResponse(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestResponse> { Success = false, Message = "Lỗi lấy harvest", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<HarvestSummary>>> GetByPlotIdAsync(Guid plotId)
        {
            try
            {
                var items = await _harvestRepo.GetByPlotIdAsync(plotId);
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = true, Data = items.Select(HarvestMapper.ToSummary) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<IEnumerable<HarvestSummary>>> GetBySeasonIdAsync(Guid seasonId)
        {
            try
            {
                var items = await _harvestRepo.GetBySeasonIdAsync(seasonId);
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = true, Data = items.Select(HarvestMapper.ToSummary) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<HarvestSummary>> { Success = false, Message = "Lỗi", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ApiResponse<HarvestResponse>> CreateAsync(CreateHarvestRequest request)
        {
            try
            {
                var plot = await _plotRepo.GetByIdAsync(request.PlotId);
                if (plot == null)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Plot không tồn tại" };

                var season = await _seasonRepo.GetByIdAsync(request.SeasonId);
                if (season == null)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Season không tồn tại" };

                var crop = await _cropRepo.GetByIdAsync(request.CropId);
                if (crop == null)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Crop không tồn tại" };

                var status = string.IsNullOrWhiteSpace(request.Status) ? "planned" : request.Status.Trim().ToLowerInvariant();
                if (!ValidStatuses.Contains(status))
                    return new ApiResponse<HarvestResponse> { Success = false, Message = $"Status không hợp lệ: {status}" };

                var beds = (await _bedRepo.GetBedsByPlotIdAsync(request.PlotId)).ToList();
                if (beds.Count == 0)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Plot chưa có bed nào để tạo harvest detail" };

                var now = DateTime.UtcNow;
                var harvest = new Harvest
                {
                    HarvestId = Guid.NewGuid(),
                    PlotId = request.PlotId,
                    SeasonId = request.SeasonId,
                    CropId = request.CropId,
                    ExpectedDate = request.ExpectedDate,
                    ExpectedQuantity = request.ExpectedQuantity,
                    Unit = request.Unit,
                    Status = status,
                    Notes = request.Notes,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                await _harvestRepo.AddAsync(harvest);

                var details = beds.Select(b => new HarvestDetail
                {
                    HarvestDetailId = Guid.NewGuid(),
                    HarvestId = harvest.HarvestId,
                    BedId = b.BedId,
                    CropQuantity = b.PlantCount ?? b.CropQuantities,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                }).ToList();

                await _detailRepo.AddRangeAsync(details);

                if (!await _harvestRepo.SaveChangesAsync())
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Lưu thất bại" };

                var saved = await _harvestRepo.GetByIdAsync(harvest.HarvestId);
                return new ApiResponse<HarvestResponse>
                {
                    Success = true,
                    Message = $"Đã tạo harvest và spread thành {details.Count} harvest_detail",
                    Data = saved != null ? HarvestMapper.ToResponse(saved) : HarvestMapper.ToResponse(harvest)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestResponse>
                {
                    Success = false,
                    Message = "Lỗi tạo harvest",
                    Errors = new List<string> { ex.InnerException?.Message ?? ex.Message }
                };
            }
        }

        public async Task<ApiResponse<HarvestResponse>> UpdateAsync(Guid id, UpdateHarvestRequest request)
        {
            try
            {
                var entity = await _harvestRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<HarvestResponse> { Success = false, Message = "Không tìm thấy harvest" };

                if (request.ExpectedDate.HasValue) entity.ExpectedDate = request.ExpectedDate;
                if (request.ExpectedQuantity.HasValue) entity.ExpectedQuantity = request.ExpectedQuantity;
                if (request.Unit != null) entity.Unit = request.Unit;
                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    var status = request.Status.Trim().ToLowerInvariant();
                    if (!ValidStatuses.Contains(status))
                        return new ApiResponse<HarvestResponse> { Success = false, Message = $"Status không hợp lệ: {status}" };
                    entity.Status = status;
                }
                if (request.Notes != null) entity.Notes = request.Notes;
                entity.UpdatedAt = DateTime.UtcNow;

                _harvestRepo.Update(entity);
                await _harvestRepo.SaveChangesAsync();

                return new ApiResponse<HarvestResponse>
                {
                    Success = true,
                    Message = "Cập nhật harvest thành công",
                    Data = HarvestMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HarvestResponse>
                {
                    Success = false,
                    Message = "Lỗi cập nhật harvest",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _harvestRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Không tìm thấy harvest" };

                _harvestRepo.Delete(entity);
                await _harvestRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Đã xóa harvest và toàn bộ detail/record liên quan" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = "Lỗi xóa harvest", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
