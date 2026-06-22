using TestAPI.DTO.Auth.Requests;
using TestAPI.Models;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result> RegisterUser(RegisterUserRequest registerUserRequest, CancellationToken ct = default);
        Task<Result<TokenResponse>> LoginUser(LoginUserRequest loginUserRequest, CancellationToken ct = default);
    }
}
