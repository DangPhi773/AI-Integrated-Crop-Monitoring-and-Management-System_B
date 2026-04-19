using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IDiagnosisResultRepository
    {
        Task<List<DiagnosisResult>> GetAllWithDetailsAsync();
        Task<DiagnosisResult?> GetByIdWithDetailsAsync(Guid id);
        Task<List<DiagnosisResult>> GetByReportIdAsync(Guid reportId);
        System.Threading.Tasks.Task AddAsync(DiagnosisResult entity);
        Task<bool> SaveChangesAsync();
    }
}
