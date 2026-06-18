using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.DTO;
namespace TestAPI.Services.Interfaces
{
    public interface IJWTService
    {
        public Task<TokenResponse> ProvideTokens(User user);
         public Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
