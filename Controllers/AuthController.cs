using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Models;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;
using TestAPI.Validation.Authentication;

namespace TestAPI.Controllers
{

    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IValidatorResolver _validatorResolver;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        private readonly IValidator<RegisterUserRequest> _registerUserRequestValidator;
        private readonly IValidator<LoginUserRequest> _loginUserRequestValidator;
        public AuthController( 
            IValidatorResolver validatorResolver,
            IUserService userService,
             IJWTService jWTService,
             IAuthService authService, 
             IValidator<RegisterUserRequest> registerUserRequestValidator,
             IValidator<LoginUserRequest> loginUserRequestValidator
             )
        {
            _validatorResolver = validatorResolver;
            _registerUserRequestValidator = registerUserRequestValidator;
            _loginUserRequestValidator = loginUserRequestValidator;
            _userService = userService;
            _jwtService = jWTService;
            _authService = authService;
        }

        [HttpPost("/api/auth/register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
        {
            var validationResult = await _validatorResolver.ValidateAsync(registerUserRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _authService.RegisterUser(registerUserRequest);
            return Ok();
        }

        [HttpPost("/api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            var validationResult = await _loginUserRequestValidator.ValidateAsync(loginUserRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var tokens = await _authService.LoginUser(loginUserRequest);
            return Ok(tokens);
        }

        [HttpPost("api/auth/refresh")]
        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            // validation
            return await _jwtService.RefreshToken(request);
        }



    }
}
