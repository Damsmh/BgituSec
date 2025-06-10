using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task UpdateAsync(User user);
        public Task DeleteAsync(int id);
        public Task<User?> GetByIdAsync(int id);
        public Task<User?> GetByNameAsync(string name);
        public Task<ICollection<User>> GetAllAsync();

    }
}
