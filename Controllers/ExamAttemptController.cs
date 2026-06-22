using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [ApiController]
  [Authorize]
  public class ExamAttemptController : ControllerBase
  {
    private readonly IExamAttemptService _examAttemptService;

    public ExamAttemptController(IExamAttemptService examAttemptService)
    {
      _examAttemptService = examAttemptService;
    }

    [HttpPost("/api/session/start")]
    public async Task<IActionResult> StartSession([FromQuery] Guid examId, CancellationToken ct)
    {
      var userId = User.GetUserId();
      if (userId == null)
      {
        return Unauthorized();
      }

      var compilationResult = await _examAttemptService.StartSession(userId.Value, examId, ct);
      return compilationResult.IsSuccess ? Ok(compilationResult.Value) : compilationResult.ToActionResult();
    }

    [HttpPost("/api/session/finish")]
    public async Task<IActionResult> FinishSession(
        [FromBody] FinishSessionRequest request,
        CancellationToken ct)
    {
      var userId = User.GetUserId();
      if (userId == null)
      {
        return Unauthorized();
      }

      var result = await _examAttemptService.FinishSession(request, userId.Value, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("/api/attempts/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      var result = await _examAttemptService.DeleteAsync(id);
      return result.ToActionResult();
    }
  }
}
