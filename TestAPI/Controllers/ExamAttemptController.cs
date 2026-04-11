using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("/api/attempts/{id}/start")]
        public async Task<IActionResult> StartExam([FromQuery] Guid id)
        {
            var userId = Guid.NewGuid();
            var examAttemptId = await _examAttemptService.StartAttemptAsync(userId, id);

            return Ok(examAttemptId);
        }

        [HttpPost("/api/attempts/save")]

        public async Task<IActionResult> SaveResponse(SaveResponseRequest response)
        {
            await _examAttemptService.SaveResponse(response.ExamAttemptId, response.QuestionId, response.SelectedOptionId);
            return Ok();
        }


        [HttpPost("/api/attempts/{id}/finish")]

        public async Task<IActionResult> FinishExam(Guid id)
        {
            await _examAttemptService.FinishAttemptAsync(id);
            return Ok();
        }



        // [HttpPost("/api/attempts/{id}/responses")]
        // public async Task<IActionResult> GetResponsesByAttemptId(Guid examAttemptId){

        //     var userExamResponses = await _examAttemptService.GetResponsesAsync(examAttemptId);
        //     return Ok(userExamResponses);
        // }

        [HttpGet("/{attemptId}")]

        public async Task<ActionResult<ExamAttemptDto>> GetAttemptById(Guid attemptId)
        {

            var examAttempt = await _examAttemptService.GetByIdAsync(attemptId);
            return Ok(examAttempt);
        }



        // [HttpGet("/api/attempts/{id}/user")]

        // public async Task<ActionResult<ExamAttemptDto>> GetAttemptByUserId(Guid userId){

        //     // var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //     var examAttempt = await _examAttemptService.GetByUserId(userId);
        //     return Ok(examAttempt);
        // }
        [HttpDelete("/api/attempts/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            await _examAttemptService.DeleteAsync(id);
            return Ok();
        }



    }
}

