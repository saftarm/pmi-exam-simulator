using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestAPI.DTO;
using TestAPI.Extensions;
using TestAPI.Models.Pagination;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
namespace TestAPI.Controllers
{
  [ApiController]
  public class ExamAttemptController : ControllerBase
  {
    private readonly IExamAttemptService _examAttemptService;
    private readonly IExamCacheService _examCacheService;

    public ExamAttemptController(
        IExamCacheService examCacheService,
        IExamAttemptService examAttemptService
        )
    {
      _examCacheService = examCacheService;
      _examAttemptService = examAttemptService;
    }

    // Start Exam Session
    [HttpPost("/api/session/start")]
    public async Task<IActionResult> StartSession(Guid userId, Guid examId)
    {
     var compilationResult = await _examAttemptService.StartSession(userId: userId, examId: examId);

      return compilationResult.IsSuccess ? Ok(compilationResult.Value) : compilationResult.ToActionResult();
    }


    // Finish Exam Session
    [HttpPost("/api/session/finish")]
    public async Task<IActionResult> FinishSession([FromBody] FinishSessionRequest request, CancellationToken ct) {
      // validation
      var result = await _examAttemptService.FinishSession(request, ct);
      return result.IsSuccess ?  Ok(result.Value) : result.ToActionResult();
    }

    [HttpGet("/api/session/{sessionId}/questions")]
    public async Task<IActionResult> GetSessionQuestions([FromRoute] Guid sessionId, [FromQuery] PageParameters pageParameters) {
      var result = await _examCacheService.QueryPaginatedQuestionsBySessionId(sessionId: sessionId,pageParameters: pageParameters);

      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }


    [HttpGet("api/session/questions/count")]
    public async Task<IActionResult> GetNumberOfQuestions(Guid sessionId) {
      var result = await _examCacheService.NumberOfQuestionsBySessionId(sessionId);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

   // Delete Attempt
    [HttpDelete("/api/attempts/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _examAttemptService.DeleteAsync(id);
      return Ok();
    }

  }
}

