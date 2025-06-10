using BgituSec.Domain.Entities;

namespace BgituSec.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        public Task AddAsync(RefreshToken token);
        public Task<RefreshToken?> GetAsync(int userId);
        public Task UpdateAsync(RefreshToken token);
    }
}
