using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [ApiController]
  public class ExamAttemptController : ControllerBase
  {
    private readonly IExamAttemptService _examAttemptService;

    public ExamAttemptController(IExamAttemptService examAttemptService)
    {
      _examAttemptService = examAttemptService;
    }

    [HttpPost("/api/session/start")]
    public async Task<IActionResult> StartSession(Guid userId, Guid examId)
    {
      var compilationResult = await _examAttemptService.StartSession(userId: userId, examId: examId);
      return compilationResult.IsSuccess ? Ok(compilationResult.Value) : compilationResult.ToActionResult();
    }

    [HttpPost("/api/session/finish")]
    public async Task<IActionResult> FinishSession(
        [FromBody] FinishSessionRequest request,
        CancellationToken ct)
    {
      var result = await _examAttemptService.FinishSession(request, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpDelete("/api/attempts/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _examAttemptService.DeleteAsync(id);
      return Ok();
    }
  }
}
