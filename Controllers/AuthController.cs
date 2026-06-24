using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Auth.Requests;
using TestAPI.DTO.User;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidatorResolver _validatorResolver;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;

        public AuthController(
            IValidatorResolver validatorResolver,
            IUserService userService,
            IJWTService jWTService,
            IAuthService authService)
        {
            _validatorResolver = validatorResolver;
            _userService = userService;
            _jwtService = jWTService;
            _authService = authService;
        }

        [HttpPost("/api/auth/register")]
        public async Task<IActionResult> Register(
            RegisterUserRequest registerUserRequest,
            CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(registerUserRequest);
            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationActionResult();
            }

            var result = await _authService.RegisterUser(registerUserRequest, ct);
            return result.IsSuccess ? Created("/api/auth/register", null) : result.ToActionResult();
        }

        [HttpPost("/api/auth/login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserRequest loginUserRequest,
            CancellationToken ct)
        {
            var validationResult = await _validatorResolver.ValidateAsync(loginUserRequest);
            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationActionResult();
            }

            var result = await _authService.LoginUser(loginUserRequest, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpPost("/api/auth/refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _jwtService.RefreshToken(request);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [Authorize]
        [HttpGet("/api/auth/me")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _userService.GetByIdAsync(userId.Value, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [Authorize]
        [HttpPatch("/api/auth/me")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileRequest request,
            CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdateProfileAsync(userId.Value, request, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }
    }
}
