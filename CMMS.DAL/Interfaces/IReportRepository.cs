using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid id);
        Task<IEnumerable<Report>> GetAllWithDetailsAsync();
        Task<Report?> GetByIdWithDetailsAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Report report);
        void Update(Report report);
        void Delete(Report report);
        Task<bool> SaveChangesAsync();
    }
}
