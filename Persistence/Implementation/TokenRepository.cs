using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId)
        {

            return await _context.RefreshTokens
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(rt => rt.UserId == userId);
        }

        public Task RevokeRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return Task.CompletedTask;
        }

        public async Task SaveRefreshToken(RefreshToken newRefreshToken)
        {
            await _context.RefreshTokens.AddAsync(newRefreshToken);
        }

        public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct)
        {
            await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.Revoked)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Revoked, true), ct);
        }

    }
}
