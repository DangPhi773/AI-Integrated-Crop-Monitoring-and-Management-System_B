using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IGrowthTrackingRepository
    {
        Task<IEnumerable<GrowthTracking>> GetAllAsync();
        Task<GrowthTracking?> GetByIdAsync(Guid id);
        Task<IEnumerable<GrowthTracking>> GetByHarvestDetailIdAsync(Guid harvestDetailId);
        Task<IEnumerable<GrowthTracking>> GetBySeasonIdAsync(Guid seasonId);
        Task<GrowthTracking?> GetCurrentByHarvestDetailIdAsync(Guid harvestDetailId);
        System.Threading.Tasks.Task AddAsync(GrowthTracking tracking);
        void Update(GrowthTracking tracking);
        void Delete(GrowthTracking tracking);
        Task<bool> SaveChangesAsync();
    }
}
