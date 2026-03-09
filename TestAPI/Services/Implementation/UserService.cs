using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IJWTService jwtService;

        public UserService(IUserRepository userRepository, IJWTService jWTService)
        {
            _userRepository = userRepository;
            jwtService = jWTService;
        }

        // public Task RegisterUser(RegisterUserRequest registerUserRequest)
        // {
        //     var newUser = new User
        //     {
                
        //     };

        //     _userRepository.AddAsync(newUser);

        // }

        
    }

}
