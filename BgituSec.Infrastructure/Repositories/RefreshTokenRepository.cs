using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using BgituSec.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BgituSec.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(RefreshToken token)
        {
            await _dbContext.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetAsync(int userId)
        {
            var result = await _dbContext.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.IsRevoked == false)
                .OrderBy(rt => rt.ExpiresAt)
                .LastOrDefaultAsync();
            return result;
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}
