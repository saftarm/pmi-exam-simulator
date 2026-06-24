using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet("domains")]
        public async Task<IActionResult> GetUserDomainPerformances(CancellationToken ct)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _progressService.GetUserDomainPerformancesAsync(userId.Value, ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }
    }
}
