using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ISoilRepository
    {
        Task<IEnumerable<Soil>> GetAllAsync();
        Task<Soil?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Soil soil);
        void Update(Soil soil);
        void Delete(Soil soil);
        Task<bool> SaveChangesAsync();
    }
}
