using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Exam.Requests;
using TestAPI.Extensions;
using TestAPI.Models.Pagination;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [AllowAnonymous]
        [HttpGet("/api/exams/{examId}/details")]
        public async Task<IActionResult> GetExamDetails(Guid examId)
        {
            var result = await _examService.GetDetailsByIdAsync(examId);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [AllowAnonymous]
        [HttpGet("/api/exams/details")]
        public async Task<IActionResult> GetExamsWithDetails([FromQuery] PageParameters pageParameters)
        {
            var result = await _examService.GetPublishedExamsDetailsAsync(pageParameters);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpGet("/api/exams")]
        public async Task<IActionResult> GetAllExams(CancellationToken ct)
        {
            var result = await _examService.GetAllExamsAsync(ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }

        [HttpPatch("/api/exams/{id}/update")]
        public async Task<IActionResult> UpdateExam([FromRoute] Guid id, UpdateExamRequest request)
        {
            var result = await _examService.UpdateAsync(id, request);
            return result.ToActionResult();
        }

        [HttpDelete("/api/exams/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _examService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("/api/exams/{id}/publish")]
        public async Task<IActionResult> PublishExam(Guid id)
        {
            var result = await _examService.PublishExam(id);
            return result.ToActionResult();
        }

        [HttpDelete("/api/exams")]
        public async Task<IActionResult> DeleteRange([FromBody] BulkDeleteExamsRequest request)
        {
            await _examService.DeleteRangeAsync(request.ExamIds);
            return NoContent();
        }

        [HttpPost("/api/exams")]
        public async Task<IActionResult> Create([FromBody] CreateExamDto dto, CancellationToken ct)
        {
            var result = await _examService.CreateExamAsync(dto, ct);
            return result.IsSuccess ? Created("/api/exams", null) : result.ToActionResult();
        }

        [HttpPost("/api/exams/{id}/archive")]
        public async Task<IActionResult> ArchiveExam(Guid id)
        {
            var result = await _examService.ArchiveAsync(id);
            return result.ToActionResult();
        }

        [HttpGet("/api/admin/exams/stats")]
        public async Task<IActionResult> GetExamOverviewStats(CancellationToken ct)
        {
            var result = await _examService.GetExamOverviewStatsAsync(ct);
            return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
        }
    }
}
