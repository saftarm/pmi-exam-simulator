using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Entities;
using TestAPI.Exceptions;
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
        private readonly IValidator<LoginUserRequest> _loginUserRequestValidator;
        public AuthService(IUserRepository userRepository,
            IJWTService jwtService,
            IPasswordHasher<User> passwordHasher,
            IValidator<LoginUserRequest> loginUserRequestValidator
            
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _loginUserRequestValidator = loginUserRequestValidator;
        }


        public async Task RegisterUser(RegisterUserRequest registerUserRequest)
        {
            if (registerUserRequest == null)
            {
                throw new ArgumentNullException(nameof(registerUserRequest), "Invalid data input");
            }

            if (registerUserRequest.UserName == null || registerUserRequest.Password == null
            || registerUserRequest.FirstName == null || registerUserRequest.Email == null)
            {

                throw new ArgumentException("Sign Up credentials");
            }

            var newUser = new User
            {

                UserName = registerUserRequest.UserName,
                FirstName = registerUserRequest.FirstName,
                Email = registerUserRequest.Email,
                DisplayName = registerUserRequest.UserName

            };
            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, registerUserRequest.Password);
            newUser.PasswordHash = hashedPassword;
            await _userRepository.AddAsync(newUser);
        }

        public async Task<TokenResponse> LoginUser(LoginUserRequest loginUserRequest)
        {
            // _loginUserRequestValidator.ValidateAndThrow(loginUserRequest);

            var userInDb = await _userRepository.GetByUserNameAsync(loginUserRequest.UserName!);

            if (userInDb == null)
            {
                throw new RecordNotFoundException("User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.PasswordHash, loginUserRequest.Password!);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid password");
            }
            var tokens = await _jwtService.ProvideTokens(userInDb);

            return tokens;
        }
    }
}
