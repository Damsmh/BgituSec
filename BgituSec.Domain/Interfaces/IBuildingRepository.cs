using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IBuildingRepository
    {
        public Task<ICollection<Building>> GetAllAsync();
        public Task<Building> GetByNumber(int number);
        public Task<Building> GetByIdAsync(int id);
        public Task AddAsync(Building building);
        public Task UpdateAsync(Building building);
        public Task DeleteAsync(int id);
    }
}
