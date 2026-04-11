using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IDiagnosisResultRepository
    {
        System.Threading.Tasks.Task AddAsync(DiagnosisResult entity);
        Task<bool> SaveChangesAsync();
    }
}
