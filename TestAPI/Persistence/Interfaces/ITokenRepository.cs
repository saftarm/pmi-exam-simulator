using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface ITokenRepository
    {
        public Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId);
        public Task SaveRefreshToken(RefreshToken newRefreshToken);
        public Task RevokeRefreshToken(RefreshToken refreshToken);

    }
}