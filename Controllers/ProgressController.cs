using Microsoft.AspNetCore.Mvc;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProgressController : ControllerBase
  {
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
      _progressService = progressService;
    }

    [HttpGet("domains")]
    public async Task<IActionResult> GetUserDomainPerformances([FromQuery] Guid userId, CancellationToken ct)
    {
      var result = await _progressService.GetUserDomainPerformancesAsync(userId, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }
  }
}
