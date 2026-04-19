using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<IEnumerable<Recommendation>> GetAllWithDetailsAsync();
        Task<Recommendation?> GetByIdWithDetailsAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(Recommendation recommendation);
        void Update(Recommendation recommendation);
        void Delete(Recommendation recommendation);
        Task<bool> SaveChangesAsync();
    }
}
