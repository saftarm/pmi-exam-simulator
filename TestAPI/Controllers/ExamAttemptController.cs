using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;
using TestAPI.DTO;
using TestAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Build.Experimental.BuildCheck;
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
        public async Task<IActionResult> StartExam(int id) {

    
            var userId = 1;
            var examAttemptId = await _examAttemptService.StartAttemptAsync(userId, id);

            return Ok(examAttemptId);
        }

        [HttpPost("/api/attempts/save")]

        public async Task<IActionResult> SaveResponse(SaveResponseRequest response) {
            await _examAttemptService.SaveResponse(response.ExamAttemptId, response.QuestionId, response.SelectedOptionId);
            return Ok();
        }

        
        [HttpPost("/api/attempts/{id}/finish")]

        public async Task<IActionResult> FinishExam (int id) {
            await _examAttemptService.FinishAttemptAsync(id);
            return Ok();
        }



        [HttpPost("/api/attempts/{id}/responses")]
        public async Task<IActionResult> GetResponsesByAttemptId(int examAttemptId){

            var userExamResponses = await _examAttemptService.GetResponsesAsync(examAttemptId);
            return Ok(userExamResponses);
        }

        [HttpGet("/{attemptId}")]

        public async Task<ActionResult<ExamAttemptDto>> GetAttemptById(int attemptId){

            var examAttempt = await _examAttemptService.GetByIdAsync(attemptId);
            return Ok(examAttempt);
        }



        // [HttpGet("/api/attempts/{id}/user")]

        // public async Task<ActionResult<ExamAttemptDto>> GetAttemptByUserId(int userId){

        //     // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
        //     var examAttempt = await _examAttemptService.GetByUserId(userId);
        //     return Ok(examAttempt);
        // }
        [HttpDelete("/api/attempts/{id}")]

        public async Task<IActionResult> Delete(int id) {
            await _examAttemptService.DeleteAsync(id);
            return Ok();
        }


        
    }
}

