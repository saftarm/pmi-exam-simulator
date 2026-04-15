using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.DTO.Exam.Requests;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        private readonly ApplicationDbContext _context;
        public ExamController(IExamService examService, ApplicationDbContext context)
        {
            _examService = examService;
            _context = context;
        }

        [HttpGet("/api/exams")]
        public async Task<IActionResult> GetPublishedExams([FromQuery] Guid categoryId, [FromQuery] PageParameters pageParameters, CancellationToken ct)
        {
            var exams = await _examService.GetPublishedExamSummariesByCategoryId(categoryId, pageParameters, ct);

            return Ok(exams);
        }

        [HttpPost("/api/exams")]
        public async Task<ActionResult<IEnumerable<ExamSummaryDto>>> Create([FromBody] List<CreateExamDto> dto)
        {
            var createdExams = await _examService.CreateExams(dto);
            return Ok(createdExams);
        }

        [HttpPost("/api/exams/{id}/compile")]
        public async Task<ActionResult<Exam>> CompileExam(Guid id)
        {
            await _examService.CompileExam(id);
            return Ok();
        }

        // // GET: api/Exam/Summary
        [HttpGet("/api/exams/summary")]
        public async Task<ActionResult<IEnumerable<ExamSummaryDto>>> GetSummariesAsync([FromQuery] PageParameters pageParameters)
        {
            var examsSummaries = await _examService.GetSummariesAsync(pageParameters);

            return Ok(examsSummaries);
        }
        [HttpGet("/api/exams/{id}/summary")]
        public async Task<ActionResult<ExamSummaryDto>> GetSummaryByIdAsync(Guid id)
        {
            var examSummary = await _examService.GetSummaryByIdAsync(id);
            return Ok(examSummary);
        }


        [HttpGet("/api/exams/details")]
        public async Task<ActionResult<IEnumerable<ExamDetailsDto>>> GetDetailsAsync([FromQuery] PageParameters pageParameters)
        {
            var examDetails = await _examService.GetDetailsAsync(pageParameters);

            return Ok(examDetails);
        }



        [HttpGet("/api/exams/{id:int}/details")]

        public async Task<ActionResult<ExamDetailsDto>> GetDetailsByIdAsync(Guid id)
        {

            var examDetails = await _examService.GetDetailsByIdAsync(id);

            if (examDetails == null)
            {
                return NotFound($"Exam with Id: {id} not found");
            }
            return Ok(examDetails);
        }






        [HttpDelete("api/exams/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            await _examService.DeleteAsync(id);

            return NoContent();
        }
        [HttpDelete("api/exams")]
        public async Task<IActionResult> DeleteRange(DeleteExamsRequest request)
        {

            if (request.ExamIds == null)
            {
                throw new ArgumentNullException("Invalid Exam Id input");
            }

            await _examService.DeleteRangeAsync(request.ExamIds);
            return NoContent();
        }

        [HttpPost("api/exams/{id}/publish")]

        public async Task<IActionResult> PublishExam(Guid id)
        {
            await _examService.PublihExam(id);

            return Ok();
        }


        [HttpPatch("api/exams/{id}/update")]
        public async Task<ExamSummaryDto> UpdateExam([FromRoute] Guid id, UpdateExamRequest request)
        {
            var updatedExam = await _examService.UpdateAsync(id, request);

            return updatedExam;
        }




    }
}

