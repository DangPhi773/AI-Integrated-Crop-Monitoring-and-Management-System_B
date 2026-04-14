using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMMS.DAL.Repositories
{
    public class IotDataRepository : IIotDataRepository
    {
        private readonly AppDbContext _context;
        public IotDataRepository(AppDbContext context) => _context = context;

        public async System.Threading.Tasks.Task<IEnumerable<IotData>> GetAllAsync()
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IotData?> GetByIdAsync(Guid id)
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .FirstOrDefaultAsync(d => d.SensorDataId == id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<IotData>> GetByDeviceIdAsync(Guid deviceId)
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                .Include(d => d.Season)
                .Where(d => d.DeviceId == deviceId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IotData?> GetLatestByDeviceIdAsync(Guid deviceId)
        {
            return await _context.IotDatas
                .Where(d => d.DeviceId == deviceId)
                .OrderByDescending(d => d.RecordedAt)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async System.Threading.Tasks.Task<List<IotData>> GetLatestByBedIdAsync(Guid bedId, int count)
        {
            return await _context.IotDatas
                .AsNoTracking()
                .Include(d => d.Device)
                .Where(d => d.Device != null && d.Device.BedId == bedId)
                .OrderByDescending(d => d.RecordedAt)
                .Take(count)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<List<IotData>> GetHistoryAsync(Guid deviceId, DateTime from, DateTime to)
        {
            return await _context.IotDatas
                .Where(d => d.DeviceId == deviceId && d.RecordedAt >= from && d.RecordedAt <= to)
                .OrderByDescending(d => d.RecordedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<List<IotData>> GetAlertsByFarmIdAsync(Guid farmId)
        {
            return await _context.IotDatas
                .Include(d => d.Device)
                    .ThenInclude(dev => dev!.Bed)
                        .ThenInclude(bed => bed!.Plot)
                .Where(d => d.IsAlert
                    && d.Device != null
                    && d.Device.Bed != null
                    && d.Device.Bed.Plot != null
                    && d.Device.Bed.Plot.FarmId == farmId)
                .OrderByDescending(d => d.RecordedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(IotData entity) => await _context.IotDatas.AddAsync(entity);

        public void Delete(IotData entity) => _context.IotDatas.Remove(entity);

        public async System.Threading.Tasks.Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
