using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;
        public ProgressController(ApplicationDbContext context, IProgressService progressService)
        {
            _progressService = progressService;
        }

        // Get User Progress by Id
        [Authorize]
        [HttpGet("/api/user/progress")]
        public async Task<ActionResult<ExamProgressSummaryDto>> GetProgress([FromQuery] Guid examId, CancellationToken ct)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }
            var examProgressSummary = await _progressService.GetExamProgressSummaryAsync(userId, examId, ct);
            return Ok(examProgressSummary);

        }
    }
}
