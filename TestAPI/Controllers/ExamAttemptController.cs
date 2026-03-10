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

        [HttpPost("start/{examId}")]
        public async Task<IActionResult> StartExam(int examId) {


            var userId = 3;
            var examAttemptId = await _examAttemptService.StartAttemptAsync(userId, examId);

            return Ok($"Exam Started, ID:{examAttemptId}");
        }



        // [HttpPost("finish/")]

        // public async Task<IActionResult> FinishAttempt([FromBody] FinishExamAttemptRequest finishExamAttemptRequest)
        // {
        //     var examAttemptId = await _examAttemptService.FinishAttemptAsync(finishExamAttemptRequest.ExamAttemptId, finishExamAttemptRequest.userExamResponses);
        //     return Ok(examAttemptId);
        // }








        // [HttpPost("finish/")]
        // public async Task FinishExam(FinishExamRequest request) {
        //     return await _examAttemptService.FinishExam(request);   
        // }

    // // [Authorize]
    // [HttpPost("finish/{examAttemptId}")]

        // public async Task<IActionResult> FinishExam(int examAttemptId) {
        //     var examAttempt = await _examAttemptService.FinishAttempt(examAttemptId);
        //     return Ok(examAttempt);
        // }

        [HttpGet("ExamAttempt/{attemptId}")]

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

