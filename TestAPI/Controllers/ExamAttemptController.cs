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
    [Route("api/[controller]")]
    [ApiController]
    public class ExamAttemptController : ControllerBase
    {
  
        private readonly IExamAttemptService _examAttemptService;

        public ExamAttemptController(IExamAttemptService examAttemptService)
        {
            _examAttemptService = examAttemptService;
        }

        // ----  Actions -----

        [HttpPost("{examId:int}")]
        public async Task<IActionResult> StartExam(int examId) {

    
            var userId = 3;
            var examAttemptId = await _examAttemptService.StartAttemptAsync(userId, examId);

            return Ok($"Exam Started, ID:{examAttemptId}");
        }

        [HttpPost("save/")]

        public async Task<IActionResult> SaveResponse(SaveResponseRequest response) {
            await _examAttemptService.SaveResponse(response.ExamAttemptId, response.QuestionId, response.SelectedOptionId);
            return Ok();
        }

        
        [HttpPost("finish/{examAttemptId}")]

        public async Task<IActionResult> FinishExam (int examAttemptId) {
            await _examAttemptService.FinishAttemptAsync(examAttemptId);
            return Ok();
        }



        [HttpPost("responses/{examAttemptId}")]
        public async Task<IActionResult> GetResponsesByAttemptId(int examAttemptId){

            var userExamResponses = await _examAttemptService.GetResponsesAsync(examAttemptId);
            return Ok(userExamResponses);
        }

        [HttpGet("/{attemptId}")]

        public async Task<ActionResult<ExamAttemptDto>> GetAttemptById(int attemptId){

            var examAttempt = await _examAttemptService.GetByIdAsync(attemptId);
            return Ok(examAttempt);
        }



        [HttpGet("user/{userId}")]

        public async Task<ActionResult<ExamAttemptDto>> GetAttemptByUserId(int userId){

            // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var examAttempt = await _examAttemptService.GetByUserId(userId);
            return Ok(examAttempt);
        }


        
    }
}

