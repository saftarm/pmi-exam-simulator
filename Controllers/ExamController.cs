using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using TestAPI.DTO;
using TestAPI.DTO.Exam.Requests;
using TestAPI.Extensions;
using TestAPI.Models.Pagination;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
  [ApiController]
  public class ExamController : ControllerBase
  {
    private readonly IExamService _examService;
    public ExamController(IExamService examService)
    {
      _examService = examService;
    }

    [HttpGet("/api/exams/{examId}/details")]
    public async Task<IActionResult> GetExamDetails(Guid examId)
    {
      var result = await _examService.GetDetailsByIdAsync(examId);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    // Get Paginated Exam Details 
    [HttpGet("/api/exams/details")]
    public async Task<IActionResult> GetExamsWithDetails([FromQuery] PageParameters pageParameters)
    {
      var result = await _examService.GetPublishedExamsDetailsAsync(pageParameters);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    // ---------------------------------------   Admin endpoints   -----------------------------------------------
    [HttpGet("/api/exams")]
    public async Task<IActionResult> GetAllExams()
    {
      var allExams = await _examService.GetAllExams();
      return Ok(allExams);
    }

    // Update Exam 
    [HttpPatch("api/exams/{id}/update")]
    public async Task<IActionResult> UpdateExam([FromRoute] Guid id, UpdateExamRequest request)
    {
      var result = await _examService.UpdateAsync(id, request);
      return result.IsSuccess ? NoContent() : result.ToActionResult();
    }

    // Hard Delete Exam
    [HttpDelete("api/exams/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _examService.DeleteAsync(id);

      return NoContent();
    }

    // Publish Exam by Id
    [HttpPost("api/exams/{id}/publish")]
    public async Task<IActionResult> PublishExam(Guid id)
    {
      var result = await _examService.PublishExam(id);

      return result.IsSuccess ? NoContent() : result.ToActionResult();
    }

    // Delete Multiple Exams
    [HttpDelete("api/exams")]
    public async Task<IActionResult> DeleteRange(IEnumerable<Guid> examIds)
    {
      await _examService.DeleteRangeAsync(examIds);
      return NoContent();
    }

    // Create Exam
    [HttpPost("/api/exams/")]
    public async Task<IActionResult> Create([FromBody] CreateExamDto dto)
    {
      // validation
      var result = await _examService.CreateExamAsync(dto);
      return result.IsSuccess ? Created() : result.ToActionResult();
    }
  }
}

