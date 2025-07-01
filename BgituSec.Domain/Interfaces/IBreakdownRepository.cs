using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IBreakdownRepository
    {
        public Task<Breakdown> GetByIdAsync(int id);
        public Task<ICollection<Breakdown>> GetAllAsync();
        public Task AddAsync(Breakdown breakdown);
        public Task UpdateAsync(Breakdown breakdown);
        public Task DeleteAsync(int id);
    }
}
