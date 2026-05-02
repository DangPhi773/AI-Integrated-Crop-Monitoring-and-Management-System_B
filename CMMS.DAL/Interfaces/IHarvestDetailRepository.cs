using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IHarvestDetailRepository
    {
        Task<IEnumerable<HarvestDetail>> GetAllAsync();
        Task<HarvestDetail?> GetByIdAsync(Guid id);
        Task<IEnumerable<HarvestDetail>> GetByHarvestIdAsync(Guid harvestId);
        Task<IEnumerable<HarvestDetail>> GetBySeasonIdAsync(Guid seasonId);
        System.Threading.Tasks.Task AddAsync(HarvestDetail entity);
        System.Threading.Tasks.Task AddRangeAsync(IEnumerable<HarvestDetail> entities);
        void Update(HarvestDetail entity);
        void Delete(HarvestDetail entity);
        Task<bool> SaveChangesAsync();
    }
}
