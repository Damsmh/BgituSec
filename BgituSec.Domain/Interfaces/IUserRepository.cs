using BgituSec.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
