using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IReportAssignmentRepository
    {
        System.Threading.Tasks.Task AddAsync(ReportAssignment entity);
        Task<ReportAssignment?> GetLatestByReportAndUserAsync(Guid reportId, Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
