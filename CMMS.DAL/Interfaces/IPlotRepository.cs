using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IPlotRepository
    {
        Task<IEnumerable<Plot>> GetAllAsync();
        Task<Plot?> GetByIdAsync(Guid id);
        Task<Plot?> GetByIdLightAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Plot plot);
        void Update(Plot plot);
        void Delete(Plot plot);
        Task<bool> SaveChangesAsync();
    }
}
