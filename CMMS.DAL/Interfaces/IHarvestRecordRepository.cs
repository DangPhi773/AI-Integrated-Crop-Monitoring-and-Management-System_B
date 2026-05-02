using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IHarvestRecordRepository
    {
        Task<IEnumerable<HarvestRecord>> GetAllAsync();
        Task<HarvestRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<HarvestRecord>> GetByHarvestIdAsync(Guid harvestId);
        System.Threading.Tasks.Task AddAsync(HarvestRecord entity);
        void Update(HarvestRecord entity);
        void Delete(HarvestRecord entity);
        Task<bool> SaveChangesAsync();
    }
}
