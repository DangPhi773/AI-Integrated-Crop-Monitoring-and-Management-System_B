using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IReportEnvironmentSnapshotRepository
    {
        System.Threading.Tasks.Task AddAsync(ReportEnvironmentSnapshot entity);
        Task<bool> SaveChangesAsync();
    }
}
