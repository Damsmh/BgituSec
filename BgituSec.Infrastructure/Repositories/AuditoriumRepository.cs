using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class AuditoriumRepository(AppDbContext dbContext) : IAuditoriumRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(Auditorium auditorium)
        {
            await _dbContext.Auditoriums.AddAsync(auditorium);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var auditorium = await GetByIdAsync(id);
            _dbContext.Auditoriums.Remove(auditorium);
        }

        public async Task<ICollection<Auditorium>> GetAllAsync()
        {
            return await _dbContext.Auditoriums.ToListAsync();
        }

        public async Task<Auditorium> GetByIdAsync(int id)
        {
            return await _dbContext.Auditoriums.FindAsync(id) ?? throw new KeyNotFoundException(nameof(id));
        }

        public async Task UpdateAsync(Auditorium auditorium)
        {
            var storedAuditorium = await GetByIdAsync(auditorium.Id);
            storedAuditorium.CopyChanges(auditorium);
            await _dbContext.SaveChangesAsync();
        }
    }
}
