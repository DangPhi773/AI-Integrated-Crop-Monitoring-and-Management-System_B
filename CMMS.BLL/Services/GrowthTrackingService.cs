using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.GrowthTrackings;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;

namespace CMMS.BLL.Services
{
    public class GrowthTrackingService : IGrowthTrackingService
    {
        private readonly IGrowthTrackingRepository _trackingRepo;
        private readonly IHarvestDetailRepository _harvestDetailRepo;
        private readonly ISeasonRepository _seasonRepo;
        private readonly ICropGrowthStageRepository _stageRepo;

        public GrowthTrackingService(
            IGrowthTrackingRepository trackingRepo,
            IHarvestDetailRepository harvestDetailRepo,
            ISeasonRepository seasonRepo,
            ICropGrowthStageRepository stageRepo)
        {
            _trackingRepo = trackingRepo;
            _harvestDetailRepo = harvestDetailRepo;
            _seasonRepo = seasonRepo;
            _stageRepo = stageRepo;
        }

        public async Task<ApiResponse<IEnumerable<GrowthTrackingResponse>>> GetAllAsync()
        {
            try
            {
                var items = await _trackingRepo.GetAllAsync();
                return new ApiResponse<IEnumerable<GrowthTrackingResponse>>
                {
                    Success = true,
                    Data = items.Select(GrowthTrackingMapper.ToResponse)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GrowthTrackingResponse>>
                {
                    Success = false,
                    Message = "Error fetching growth trackings",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GrowthTrackingResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _trackingRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Growth tracking not found" };
                return new ApiResponse<GrowthTrackingResponse> { Success = true, Data = GrowthTrackingMapper.ToResponse(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = false,
                    Message = "Error fetching growth tracking",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<GrowthTrackingResponse>>> GetByHarvestDetailIdAsync(Guid harvestDetailId)
        {
            try
            {
                var items = await _trackingRepo.GetByHarvestDetailIdAsync(harvestDetailId);
                return new ApiResponse<IEnumerable<GrowthTrackingResponse>>
                {
                    Success = true,
                    Data = items.Select(GrowthTrackingMapper.ToResponse)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GrowthTrackingResponse>>
                {
                    Success = false,
                    Message = "Error fetching growth trackings",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GrowthTrackingResponse>> CreateAsync(GrowthTrackingRequest request, Guid? userId)
        {
            try
            {
                var harvestDetail = await _harvestDetailRepo.GetByIdAsync(request.HarvestDetailId);
                if (harvestDetail == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Harvest detail không tồn tại" };

                var stage = await _stageRepo.GetByIdAsync(request.StageId);
                if (stage == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Growth stage không tồn tại" };

                var harvestCropId = harvestDetail.Harvest?.CropId;
                if (harvestCropId.HasValue && stage.CropId != harvestCropId.Value)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Stage không thuộc crop của đợt thu hoạch này" };

                var existingCurrent = await _trackingRepo.GetCurrentByHarvestDetailIdAsync(request.HarvestDetailId);
                if (existingCurrent != null && existingCurrent.StageId == request.StageId)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Stage này đang được theo dõi" };

                var now = DateTime.UtcNow;
                var entity = new GrowthTracking
                {
                    TrackingId = Guid.NewGuid(),
                    HarvestDetailId = request.HarvestDetailId,
                    StageId = request.StageId,
                    StartDate = request.StartDate ?? now,
                    TrackingStatus = "In-Progress",
                    HealthStatus = request.HealthStatus,
                    ActualHeight = request.ActualHeight,
                    Notes = request.Notes,
                    LastUpdatedBy = userId,
                    LastObservedAt = now,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _trackingRepo.AddAsync(entity);
                if (!await _trackingRepo.SaveChangesAsync())
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Failed to save growth tracking" };

                var saved = await _trackingRepo.GetByIdAsync(entity.TrackingId);
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = true,
                    Message = "Growth tracking created",
                    Data = saved != null ? GrowthTrackingMapper.ToResponse(saved) : GrowthTrackingMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = false,
                    Message = "Error creating growth tracking",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GrowthTrackingResponse>> UpdateAsync(Guid id, GrowthTrackingUpdateRequest request, Guid? userId)
        {
            try
            {
                var entity = await _trackingRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Growth tracking not found" };

                if (!string.IsNullOrWhiteSpace(request.TrackingStatus)) entity.TrackingStatus = request.TrackingStatus;
                if (request.HealthStatus != null) entity.HealthStatus = request.HealthStatus;
                if (request.ActualHeight.HasValue) entity.ActualHeight = request.ActualHeight;
                if (request.ActualYield.HasValue) entity.ActualYield = request.ActualYield;
                if (request.DelayDays.HasValue) entity.DelayDays = request.DelayDays;
                if (request.DelayReason != null) entity.DelayReason = request.DelayReason;
                if (request.Notes != null) entity.Notes = request.Notes;

                entity.LastObservedAt = request.LastObservedAt ?? DateTime.UtcNow;
                entity.LastUpdatedBy = userId;
                entity.UpdatedAt = DateTime.UtcNow;

                _trackingRepo.Update(entity);
                await _trackingRepo.SaveChangesAsync();

                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = true,
                    Message = "Growth tracking updated",
                    Data = GrowthTrackingMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = false,
                    Message = "Error updating growth tracking",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GrowthTrackingResponse>> AdvanceStageAsync(Guid id, AdvanceStageRequest request, Guid? userId)
        {
            try
            {
                var current = await _trackingRepo.GetByIdAsync(id);
                if (current == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Growth tracking not found" };

                if (current.TrackingStatus == "Completed")
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Tracking hiện tại đã hoàn thành" };

                var nextStage = await _stageRepo.GetByIdAsync(request.NextStageId);
                if (nextStage == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Next stage không tồn tại" };

                var harvestDetail = await _harvestDetailRepo.GetByIdAsync(current.HarvestDetailId);
                var harvestCropId = harvestDetail?.Harvest?.CropId;
                if (harvestCropId.HasValue && nextStage.CropId != harvestCropId.Value)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Next stage không thuộc crop của đợt thu hoạch này" };

                var now = DateTime.UtcNow;
                var endDate = request.EndDate ?? now;

                current.TrackingStatus = "Completed";
                current.EndDate = endDate;
                if (request.ActualHeight.HasValue) current.ActualHeight = request.ActualHeight;
                if (request.ActualYield.HasValue) current.ActualYield = request.ActualYield;
                if (!string.IsNullOrWhiteSpace(request.Notes)) current.Notes = request.Notes;
                current.LastUpdatedBy = userId;
                current.LastObservedAt = now;
                current.UpdatedAt = now;
                _trackingRepo.Update(current);

                var next = new GrowthTracking
                {
                    TrackingId = Guid.NewGuid(),
                    HarvestDetailId = current.HarvestDetailId,
                    StageId = request.NextStageId,
                    StartDate = endDate,
                    TrackingStatus = "In-Progress",
                    LastUpdatedBy = userId,
                    LastObservedAt = now,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                await _trackingRepo.AddAsync(next);

                if (!await _trackingRepo.SaveChangesAsync())
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Failed to advance stage" };

                var saved = await _trackingRepo.GetByIdAsync(next.TrackingId);
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = true,
                    Message = "Advanced to next stage",
                    Data = saved != null ? GrowthTrackingMapper.ToResponse(saved) : GrowthTrackingMapper.ToResponse(next)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = false,
                    Message = "Error advancing stage",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<GrowthTrackingResponse>> CompleteAsync(Guid id, Guid? userId)
        {
            try
            {
                var entity = await _trackingRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<GrowthTrackingResponse> { Success = false, Message = "Growth tracking not found" };

                var now = DateTime.UtcNow;
                entity.TrackingStatus = "Completed";
                entity.EndDate = now;
                entity.LastUpdatedBy = userId;
                entity.LastObservedAt = now;
                entity.UpdatedAt = now;

                _trackingRepo.Update(entity);
                await _trackingRepo.SaveChangesAsync();

                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = true,
                    Message = "Growth tracking completed",
                    Data = GrowthTrackingMapper.ToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<GrowthTrackingResponse>
                {
                    Success = false,
                    Message = "Error completing growth tracking",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _trackingRepo.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponse<string> { Success = false, Message = "Growth tracking not found" };

                _trackingRepo.Delete(entity);
                await _trackingRepo.SaveChangesAsync();
                return new ApiResponse<string> { Success = true, Message = "Growth tracking deleted" };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Error deleting growth tracking",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<SeasonProgressResponse>> GetSeasonProgressAsync(Guid seasonId)
        {
            try
            {
                var season = await _seasonRepo.GetByIdAsync(seasonId);
                if (season == null)
                    return new ApiResponse<SeasonProgressResponse> { Success = false, Message = "Season không tồn tại" };

                var details = (await _harvestDetailRepo.GetBySeasonIdAsync(seasonId)).ToList();
                var trackings = (await _trackingRepo.GetBySeasonIdAsync(seasonId)).ToList();

                var stageCountCache = new Dictionary<Guid, int>();
                var items = new List<HarvestDetailProgressItem>();

                foreach (var hd in details)
                {
                    var hdTrackings = trackings.Where(t => t.HarvestDetailId == hd.HarvestDetailId).ToList();
                    var cropId = hd.Harvest?.CropId;
                    int totalStages = 0;
                    if (cropId.HasValue)
                    {
                        if (!stageCountCache.TryGetValue(cropId.Value, out totalStages))
                        {
                            totalStages = (await _stageRepo.GetByCropIdAsync(cropId.Value)).Count();
                            stageCountCache[cropId.Value] = totalStages;
                        }
                    }

                    var completed = hdTrackings.Count(t => t.TrackingStatus == "Completed");
                    var inProgress = hdTrackings.Count(t => t.TrackingStatus == "In-Progress");
                    var current = hdTrackings
                        .Where(t => t.TrackingStatus == "In-Progress")
                        .OrderByDescending(t => t.StartDate)
                        .FirstOrDefault();
                    var delaySum = hdTrackings.Sum(t => t.DelayDays ?? 0);
                    var lastObserved = hdTrackings
                        .Where(t => t.LastObservedAt.HasValue)
                        .Max(t => t.LastObservedAt);

                    double percent = 0;
                    if (totalStages > 0)
                        percent = Math.Round((double)completed / totalStages * 100.0, 2);

                    items.Add(new HarvestDetailProgressItem
                    {
                        HarvestDetailId = hd.HarvestDetailId,
                        BedId = hd.BedId,
                        BedName = hd.Bed?.BedName,
                        CropId = cropId,
                        CropName = hd.Harvest?.Crop?.CropName,
                        TotalStages = totalStages,
                        CompletedStages = completed,
                        InProgressStages = inProgress,
                        ProgressPercent = percent,
                        CurrentStageId = current?.StageId,
                        CurrentStageName = current?.CropGrowthStage?.StageName,
                        CurrentHealthStatus = current?.HealthStatus,
                        TotalDelayDays = delaySum,
                        LastObservedAt = lastObserved,
                        Trackings = hdTrackings.Select(GrowthTrackingMapper.ToResponse).ToList()
                    });
                }

                double overall = items.Count > 0 ? Math.Round(items.Average(i => i.ProgressPercent), 2) : 0;

                var response = new SeasonProgressResponse
                {
                    SeasonId = season.SeasonId,
                    SeasonName = season.SeasonName,
                    Status = season.Status,
                    TotalHarvestDetails = details.Count,
                    OverallProgressPercent = overall,
                    Items = items
                };

                return new ApiResponse<SeasonProgressResponse> { Success = true, Data = response };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SeasonProgressResponse>
                {
                    Success = false,
                    Message = "Error calculating season progress",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
