using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class BreakdownRepository(AppDbContext dbContext) : IBreakdownRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(Breakdown breakdown)
        {
            await _dbContext.AddAsync(breakdown);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Breakdown> DeleteAsync(int id)
        {
            var breakdown = await GetByIdAsync(id);
            _dbContext.Breakdowns.Remove(breakdown);
            await _dbContext.SaveChangesAsync();
            return breakdown;
        }

        public async Task<ICollection<Breakdown>> GetAllAsync()
        {
            return await _dbContext.Breakdowns.ToListAsync();
        }

        public async Task<Breakdown> GetByIdAsync(int id)
        {
            return await _dbContext.Breakdowns.FindAsync(id) ?? throw new KeyNotFoundException(nameof(id));
        }

        public async Task UpdateAsync(Breakdown breakdown)
        {
            var storedBreakdown = await GetByIdAsync(breakdown.Id);
            storedBreakdown.CopyChanges(breakdown);
            await _dbContext.SaveChangesAsync();
        }
    }
}
