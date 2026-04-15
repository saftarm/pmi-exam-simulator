using TestAPI.DTO.Auth.Requests;
using TestAPI.Models;
namespace TestAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task RegisterUser(RegisterUserRequest registerUserRequest);

        public Task<TokenResponse> LoginUser(LoginUserRequest loginUserRequest);

    }
}
