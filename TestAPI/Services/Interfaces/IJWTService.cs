using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.DTO;
namespace TestAPI.Services.Interfaces
{
    public interface IJWTService
    {
        public Task<TokenResponse> ProvideToken(User user);
         public Task<TokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
