using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IComputerRepository
    {
        public Task<Computer> GetByIdAsync(int id);
        public Task<ICollection<Computer>> GetAllAsync();
        public Task AddAsync(Computer computer);
        public Task UpdateAsync(Computer computer);
        public Task<Computer> DeleteAsync(int id);
    }
}
