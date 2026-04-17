using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IBedRepository
    {
        Task<IEnumerable<Bed>> GetAllAsync();
        Task<Bed?> GetByIdAsync(Guid id);
        Task<IEnumerable<Bed>> GetBedsByPlotIdAsync(Guid plotId);
        System.Threading.Tasks.Task AddAsync(Bed bed);
        void Update(Bed bed);
        void Delete(Bed bed);
        Task<bool> SaveChangesAsync();
    }
}
