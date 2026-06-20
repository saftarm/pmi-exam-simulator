using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO.Question;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [ApiController]
  [Authorize(Policy = "AdminOnly")]
  public class QuestionController : ControllerBase
  {
    private readonly IQuestionService _questionService;
    private readonly IQuestionImportService _questionImportService;

    public QuestionController(
        IQuestionService questionService,
        IQuestionImportService questionImportService)
    {
      _questionService = questionService;
      _questionImportService = questionImportService;
    }

    [HttpGet("/api/questions")]
    public async Task<IActionResult> GetQuestions(
        [FromQuery] QuestionQueryParameters query,
        CancellationToken ct)
    {
      var result = await _questionService.GetPagedAsync(query, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpPost("/api/questions/import")]
    public async Task<IActionResult> ImportQuestions([FromForm] IFormFile file, CancellationToken ct)
    {
      var result = await _questionImportService.ImportFromExcelAsync(file, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpPost("/api/questions")]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDto dto)
    {
      var result = await _questionService.CreateQuestionAsync(dto);
      return result.IsSuccess ? Created() : result.ToActionResult();
    }

    [HttpGet("/api/questions/{id:guid}")]
    public async Task<IActionResult> GetQuestionById(Guid id, CancellationToken ct)
    {
      var result = await _questionService.GetByIdAsync(id, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    [HttpPut("/api/questions/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateQuestionRequest request)
    {
      request.Id = id;
      var result = await _questionService.UpdateAsync(request);
      return result.ToActionResult();
    }

    [HttpDelete("/api/questions/{questionId:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
      var result = await _questionService.DeleteQuestionAsync(questionId);
      return result.ToActionResult();
    }

    [HttpDelete("/api/questions")]
    public async Task<IActionResult> DeleteQuestions([FromBody] BulkDeleteQuestionsRequest request)
    {
      var result = await _questionService.DeleteRangeAsync(request.QuestionIds);
      return result.ToActionResult();
    }
  }
}
