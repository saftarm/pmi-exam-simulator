using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO.User;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/api/admin/users")]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryParameters query, CancellationToken ct)
        {
            var result = await _userService.GetPagedAsync(query, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpGet("/api/admin/users/count")]
        public async Task<IActionResult> GetUserCount(CancellationToken ct)
        {
            var result = await _userService.GetTotalCountAsync(ct);
            return result.IsSuccess ? Ok(new { count = result.Value }) : result.ToActionResult();
        }

        [HttpGet("/api/admin/users/stats")]
        public async Task<IActionResult> GetUserStats(CancellationToken ct)
        {
            var result = await _userService.GetStatsAsync(ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpGet("/api/admin/users/{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id, CancellationToken ct)
        {
            var result = await _userService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpPost("/api/admin/users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var result = await _userService.CreateAsync(request, ct);
            return result.IsSuccess ? Created() : result.ToActionResult();
        }

        [HttpPut("/api/admin/users/{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct)
        {
            var actingUserId = User.GetUserId() ?? Guid.Empty;
            var result = await _userService.UpdateAsync(id, request, actingUserId, ct);
            return result.ToActionResult();
        }

        [HttpPatch("/api/admin/users/{id:guid}/status")]
        public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusRequest request, CancellationToken ct)
        {
            var actingUserId = User.GetUserId() ?? Guid.Empty;
            var result = await _userService.UpdateStatusAsync(id, request, actingUserId, ct);
            return result.ToActionResult();
        }
    }
}
