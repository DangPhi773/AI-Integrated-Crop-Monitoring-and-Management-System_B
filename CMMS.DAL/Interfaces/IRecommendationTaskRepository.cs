using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IRecommendationTaskRepository
    {
        Task<IEnumerable<RecommendationTask>> GetAllAsync();
        Task<RecommendationTask?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(RecommendationTask task);
        void Update(RecommendationTask task);
        void Delete(RecommendationTask task);
        Task<bool> SaveChangesAsync();
    }
}
