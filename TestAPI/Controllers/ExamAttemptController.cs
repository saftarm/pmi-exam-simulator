using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestAPI.DTO;
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

        // Start Attempt
        [Authorize]
        [HttpPost("/api/attempts/{id}/start")]
        public async Task<IActionResult> StartExam([FromRoute] Guid id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var examAttemptId = await _examAttemptService.StartAttemptAsync(userId, id);

            return Ok(examAttemptId);
        }

        // Save Response
        [HttpPost("/api/attempts/save")]
        public async Task<IActionResult> SaveResponse(SaveResponseRequest response)
        {
            var newResponse = await _examAttemptService.SaveResponse(response.ExamAttemptId, response.QuestionId, response.DomainId , response.SelectedOptionId);
            return Ok(newResponse);
        }


        // Finish Attempt
        [HttpPost("/api/attempts/{id}/finish")]
        public async Task<IActionResult> FinishExam(Guid id)
        {
            await _examAttemptService.FinishAttemptAsync(id);
            return Ok();
        }

        // Get Attempt by Id
        [HttpGet("/{attemptId}")]
        public async Task<ActionResult<ExamAttemptDto>> GetAttemptById(Guid attemptId)
        {

            var examAttempt = await _examAttemptService.GetByIdAsync(attemptId);
            return Ok(examAttempt);
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

