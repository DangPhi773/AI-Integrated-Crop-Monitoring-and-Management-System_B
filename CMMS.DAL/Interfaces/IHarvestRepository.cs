using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IHarvestRepository
    {
        Task<IEnumerable<Harvest>> GetAllAsync();
        Task<Harvest?> GetByIdAsync(Guid id);
        Task<IEnumerable<Harvest>> GetByPlotIdAsync(Guid plotId);
        Task<IEnumerable<Harvest>> GetBySeasonIdAsync(Guid seasonId);
        Task<IEnumerable<Harvest>> GetByFarmIdAsync(Guid farmId);
        System.Threading.Tasks.Task AddAsync(Harvest entity);
        void Update(Harvest entity);
        void Delete(Harvest entity);
        Task<bool> SaveChangesAsync();
    }
}
