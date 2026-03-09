
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.Services.Interfaces;


namespace TestAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {


        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jwtService;

        private readonly IPasswordHasher<User> _passwordHasher;



        
        public AuthService(IUserRepository userRepository, IJWTService jwtService, IPasswordHasher<User> passwordHasher) {

            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;

            
        }


        public async Task<User> RegisterUser(RegisterUserRequest registerUserRequest) {

            if( registerUserRequest.UserName == null || registerUserRequest.Password == null) {

                throw new ArgumentException("Invalid register request");
            }
            

            var newUser = new User {

                UserName = registerUserRequest.UserName,
                FirstName = registerUserRequest.FirstName,
                Email = registerUserRequest.Email,
                DisplayName = registerUserRequest.UserName
                


            };
            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser  , registerUserRequest.Password);
            newUser.PasswordHash = hashedPassword;
            await _userRepository.AddAsync(newUser);

            return newUser;
        }

        public async Task<string> LoginUser(LoginUserRequest loginUserRequest)
        {

            if(loginUserRequest.UserName == null) {

                throw new ArgumentNullException("Invalid username");
            }

            var userInDb = await _userRepository.GetByUserNameAsync(loginUserRequest.UserName);

            if( userInDb == null) {
                throw new Exception("User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.PasswordHash, loginUserRequest.Password);
            if(result == PasswordVerificationResult.Failed) {
                throw new Exception("Invalid password");
            }
            var token = _jwtService.GenerateToken(userInDb);

            return token;

        }


        





    }
}
