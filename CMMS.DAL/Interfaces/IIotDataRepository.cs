using CMMS.DAL.Entities;

namespace CMMS.DAL.Interfaces
{
    public interface IIotDataRepository
    {
        System.Threading.Tasks.Task<IEnumerable<IotData>> GetAllAsync();
        System.Threading.Tasks.Task<IotData?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<IEnumerable<IotData>> GetByDeviceIdAsync(Guid deviceId);
        System.Threading.Tasks.Task<List<IotData>> GetLatestByBedIdAsync(Guid bedId, int count);
        System.Threading.Tasks.Task AddAsync(IotData entity);
        void Delete(IotData entity);
        System.Threading.Tasks.Task<bool> SaveChangesAsync();
    }
}
