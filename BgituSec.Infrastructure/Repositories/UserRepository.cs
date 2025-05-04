using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using BgituSec.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(User user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _dbContext.Users.Where(user => user.Name == name).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var finduser = await GetByIdAsync(user.Id) ?? throw new KeyNotFoundException(nameof(user));
            finduser.CopyChanges(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
