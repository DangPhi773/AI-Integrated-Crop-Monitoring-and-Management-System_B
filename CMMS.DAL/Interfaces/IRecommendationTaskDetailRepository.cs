using CMMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Interfaces
{
    public interface IRecommendationTaskDetailRepository
    {
        Task<IEnumerable<RecommendationTaskDetail>> GetAllAsync();
        Task<IEnumerable<RecommendationTaskDetail>> GetByTaskIdAsync(Guid taskId);
        Task<RecommendationTaskDetail?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task AddAsync(RecommendationTaskDetail detail);
        void Update(RecommendationTaskDetail detail);
        void Delete(RecommendationTaskDetail detail);
        Task<bool> SaveChangesAsync();
    }
}
