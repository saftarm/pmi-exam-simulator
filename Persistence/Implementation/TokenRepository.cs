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

        public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId) {

            return await _context.RefreshTokens
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(rt => rt.UserId == userId);
        }

        public async Task RevokeRefreshToken(RefreshToken refreshToken) {
            
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

        }

        public async Task SaveRefreshToken(RefreshToken newRefreshToken) {
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();
        } 

    } 

}