using CMMS.DAL.DBContext;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly AppDbContext _context;
        public SeasonRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Season>> GetAllAsync()
            => await _context.Seasons
                .Include(s => s.Harvests)
                .Include(s => s.TaskDetails)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Season?> GetByIdAsync(Guid id)
            => await _context.Seasons
                .Include(s => s.Harvests)
                .Include(s => s.TaskDetails)
                .FirstOrDefaultAsync(s => s.SeasonId == id);

        public async System.Threading.Tasks.Task AddAsync(Season season) => await _context.Seasons.AddAsync(season);

        public void Update(Season season) => _context.Seasons.Update(season);

        public void Delete(Season season) => _context.Seasons.Remove(season);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;

    }
}
