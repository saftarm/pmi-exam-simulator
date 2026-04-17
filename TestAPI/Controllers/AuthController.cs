using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Models;
using TestAPI.Services.Interfaces;


namespace TestAPI.Controllers
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        public AuthController(IUserService userService, IJWTService jWTService, IAuthService authService)
        {
            _userService = userService;
            _jwtService = jWTService;
            _authService = authService;
        }

        [HttpPost("/api/auth/register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
        {
            await _authService.RegisterUser(registerUserRequest);
            return Ok();
        }

        [HttpPost("/api/auth/login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            var token = await _authService.LoginUser(loginUserRequest);
            return Ok(token);
        }

        [HttpPost("api/auth/refresh")]
        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request) {
            return await _jwtService.RefreshToken(request);
        }

    }
}
