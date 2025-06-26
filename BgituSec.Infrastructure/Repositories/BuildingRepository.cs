using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class BuildingRepository(AppDbContext dbContext) : IBuildingRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Building building)
        {
            await _dbContext.AddAsync(building);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var building = await GetByIdAsync(id) ?? throw new KeyNotFoundException(nameof(id)); 
            _dbContext.Remove(building);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Building?> GetByIdAsync(int id)
        {
            return await _dbContext.Buildings.FindAsync(id);
        }

        public async Task<ICollection<Building>> GetAll()
        {
            return await _dbContext.Buildings.ToListAsync();
        }

        public async Task<Building> GetByNumber(int number)
        {
            return await _dbContext.Buildings.Where(building => building.Number == number).FirstAsync();
        }

        public async Task UpdateAsync(Building building)
        {
            var oldBuilding = await GetByIdAsync(building.Id);
            oldBuilding.CopyChanges(building);
            await _dbContext.SaveChangesAsync();
        }
    }
}
