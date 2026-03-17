using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface ISeasonsDetailRepository
    {
        Task<IEnumerable<SeasonsDetail>> GetAllAsync();
        Task<SeasonsDetail?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(SeasonsDetail seasonsDetail);
        void Update(SeasonsDetail seasonsDetail);
        void Delete(SeasonsDetail seasonsDetail);
        Task<bool> SaveChangesAsync();
    }
}
