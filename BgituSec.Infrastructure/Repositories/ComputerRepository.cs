using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class ComputerRepository(AppDbContext dbContext) : IComputerRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task AddAsync(Computer computer)
        {
            await _dbContext.AddAsync(computer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Computer> DeleteAsync(int id)
        {
            var computer = await GetByIdAsync(id);
            _dbContext.Computers.Remove(computer);
            await _dbContext.SaveChangesAsync();
            return computer;
        }

        public async Task<ICollection<Computer>> GetAllAsync()
        {
            return await _dbContext.Computers.ToListAsync();
        }

        public async Task<Computer> GetByIdAsync(int id)
        {
            return await _dbContext.Computers.FindAsync(id) ?? throw new KeyNotFoundException(nameof(id));
        }

        public async Task UpdateAsync(Computer computer)
        {
            var storedComputer = await GetByIdAsync(computer.Id);
            storedComputer.CopyChanges(computer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
