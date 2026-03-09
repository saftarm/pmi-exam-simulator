using TestAPI.Models;
using TestAPI.Entities;
using TestAPI.DTO;
namespace TestAPI.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<User> RegisterUser(RegisterUserRequest registerUserRequest);

        public Task<string> LoginUser(LoginUserRequest loginUserRequest);


    }
}
