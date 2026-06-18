using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.DTO.Question;
using TestAPI.Exceptions;
using TestAPI.Extensions;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{

  [ApiController]
  public class QuestionController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IQuestionService _questionService;
    private readonly IQuestionImportService _questionImportService;
    public QuestionController(
        ApplicationDbContext context,
        IQuestionService questionService,
        IQuestionImportService questionImportService)
    {
      _context = context;
      _questionService = questionService;
      _questionImportService = questionImportService;

    }

    // Import Questions from Excel File
    [HttpPost("/api/questions/")]
    public async Task<IActionResult> ImportQuestions(Guid examId, [FromForm] IFormFile file, CancellationToken ct)
    {
      if(examId == Guid.Empty) return UnprocessableEntity("ExamId is not provided");
      var result = await _questionImportService.ImportFromExcelAsync(examId, file, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    // Get a single question by ID
    [HttpGet("/api/questions/{id}")]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(Guid id)
    {
      var question = await _questionService.GetByIdAsync(id);
      return question;
    }

    // Update an existing question
    [HttpPut("/api/questions/{id:Guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateQuestionRequest request)
    {
      request.Id = id;
      await _questionService.UpdateAsync(request);
      return NoContent();
    }

    // Hard delete question by Id
    [HttpDelete("api/questions/{questionId}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid questionId) {
      await _questionService.DeleteQuestionAsync(questionId);
      return NoContent();
      
    }
  }
}
