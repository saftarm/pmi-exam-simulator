using Microsoft.AspNetCore.Mvc;
using TestAPI.Services.Interfaces;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;


namespace TestAPI.Controllers
{

    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        private readonly IJWTService _jwtService;
       

        public AuthController(  IUserService userService, ApplicationDbContext context, IJWTService jWTService, IAuthService authService)
        {
   
            _userService = userService;
            _context = context;
            _jwtService = jWTService;
            _authService = authService;
        }

        [HttpPost("/api/auth/register")]

        public async Task<ActionResult<User>> Register(RegisterUserRequest registerUserRequest)
        {
            var newUser = await _authService.RegisterUser(registerUserRequest);
            return Ok(newUser);

        }

        [HttpPost("/api/auth/login")]
        public async Task<ActionResult<string>> Login(LoginUserRequest loginUserRequest) {

            var token = await _authService.LoginUser(loginUserRequest);
            return Ok(token);
        }

    }
}
