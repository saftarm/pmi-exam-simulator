using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IJWTService
    {
        Task<Result<TokenResponse>> ProvideTokens(User user);
        Task<Result<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request);
    }
}
