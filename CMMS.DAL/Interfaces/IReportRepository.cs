using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Report report);
        void Update(Report report);
        void Delete(Report report);
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Report>> GetAllWithWorkerAsync();
        Task<Report?> GetByIdWithWorkerAsync(Guid id);
    }
}
