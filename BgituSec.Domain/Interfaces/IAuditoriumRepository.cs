using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IAuditoriumRepository
    {
        public Task<ICollection<Auditorium>> GetAllAsync();
        public Task<Auditorium?> GetByNameAsync(string name);
        public Task<Auditorium> GetByIdAsync(int id);
        public Task AddAsync(Auditorium building);
        public Task UpdateAsync(Auditorium building);
        public Task DeleteAsync(int id);
    }
}
