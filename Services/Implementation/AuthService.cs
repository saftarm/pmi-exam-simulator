using Microsoft.AspNetCore.Identity;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ISiteSettingsRepository _siteSettingsRepository;
        private readonly IValidatorResolver _validatorResolver;

        public AuthService(
            IUserRepository userRepository,
            IJWTService jwtService,
            IPasswordHasher<User> passwordHasher,
            ISiteSettingsRepository siteSettingsRepository,
            IValidatorResolver validatorResolver)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _siteSettingsRepository = siteSettingsRepository;
            _validatorResolver = validatorResolver;
        }

        public async Task<Result> RegisterUser(RegisterUserRequest registerUserRequest, CancellationToken ct = default)
        {
            var settings = await _siteSettingsRepository.GetOrCreateAsync(ct);
            if (settings.MaintenanceMode)
            {
                return Result.Failure(Errors.MaintenanceModeActive);
            }

            if (!settings.AllowRegistration)
            {
                return Result.Failure(Errors.RegistrationDisabled);
            }

            var validationResult = await _validatorResolver.ValidateAsync(registerUserRequest);
            if (!validationResult.IsValid)
            {
                return Result.Failure(Errors.ValidationFailed);
            }

            if (!await _userRepository.IsEmailUniqueAsync(registerUserRequest.Email!, ct))
            {
                return Result.Failure(Errors.EmailAlreadyExists);
            }

            if (!await _userRepository.IsUserNameUniqueAsync(registerUserRequest.UserName!, ct))
            {
                return Result.Failure(Errors.UserNameAlreadyExists);
            }

            var newUser = new User
            {
                UserName = registerUserRequest.UserName!,
                FirstName = registerUserRequest.FirstName!,
                Email = registerUserRequest.Email!,
                DisplayName = registerUserRequest.UserName!,
                Role = UserRole.Learner,
                Status = AccountStatus.Active,
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerUserRequest.Password!);
            await _userRepository.AddAsync(newUser);

            return Result.Success();
        }

        public async Task<Result<TokenResponse>> LoginUser(
            LoginUserRequest loginUserRequest,
            CancellationToken ct = default)
        {
            var validationResult = await _validatorResolver.ValidateAsync(loginUserRequest);
            if (!validationResult.IsValid)
            {
                return Result<TokenResponse>.Failure(Errors.ValidationFailed);
            }

            var userInDb = await _userRepository.GetByUserNameAsync(loginUserRequest.UserName!);

            if (userInDb == null)
            {
                return Result<TokenResponse>.Failure(Errors.InvalidCredentials);
            }

            if (userInDb.Status != AccountStatus.Active)
            {
                return Result<TokenResponse>.Failure(Errors.AccountNotActive);
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                userInDb,
                userInDb.PasswordHash,
                loginUserRequest.Password!);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.InvalidCredentials);
            }

            var settings = await _siteSettingsRepository.GetOrCreateAsync(ct);
            if (settings.MaintenanceMode && userInDb.Role != UserRole.Admin)
            {
                return Result<TokenResponse>.Failure(Errors.MaintenanceModeActive);
            }

            return await _jwtService.ProvideTokens(userInDb);
        }
    }
}
