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
    public class ExamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IExamService _examService;

        


        public ExamController(ApplicationDbContext context, IExamService examService)
        {
            _context = context;
            _examService = examService;
        }

        // GET: api/Exam/Summary
        [HttpGet("/api/Summary")]
        public async Task<ActionResult<IEnumerable<ExamSummaryDto>>> GetSummaryAsync()
        {
            var examsSummary = await _examService.GetSummaryAsync();
            
            return Ok(examsSummary);
        }

        

        [HttpGet("/api/Details{id}")]

        public async Task<ActionResult<IEnumerable<ExamDetailsDto>>> GetDetailsByIdAsync(int id)
        {
            var examDetails = await _examService.GetDetailsByIdAsync(id);
            return Ok(examDetails);
        }



        // GET: api/Exam/5
        [HttpGet("{examId}")]
        public async Task<ActionResult<Exam>> GetExam(int examId)
        {
            var exam = await _examService.GetByIdAsync(examId);

            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }


        [HttpDelete("{examId}")]

        public async Task<ActionResult> Delete(int examId){
            await _examService.DeleteAsync(examId);

            return NoContent();
        }


        [HttpPost("CompileExam")]
        public async Task<ActionResult<Exam>> CompileExam(CompileExamDto compileExamDto)
        {
            await _examService.CompileExam(compileExamDto);
            return Ok();

        }


        public async Task<ActionResult<Exam>> CreateExam(CreateExamDto createExamDto)
        {
            var newExam = new Exam {
                Title = createExamDto.Title,
                NumberOfQuestions = createExamDto.NumberOfQuestions,
                DurationInMinutes = createExamDto.DurationInMinutes
            };

            await _context.Exams.AddAsync(newExam);
            await _context.SaveChangesAsync();

            return Ok(newExam);
        }


        [Authorize]
        [HttpPost("Start/{examId}")]
        public async Task<ActionResult<ExamAttempt>> StartExam(int examId) {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var newExamAttempt = new ExamAttempt {

                ExamId = examId,
                StartedAt = DateTime.UtcNow,
                Status = ExamStatus.InProgress,
                Score = 0,
                UserId = int.Parse(userId)

            };

            await _context.ExamAttempts.AddAsync(newExamAttempt);
            await _context.SaveChangesAsync();


            return Ok(newExamAttempt);


        }



        [HttpGet("{id}/ExamAttempt")]

        public IActionResult GetAttempt(int id) {
            var examAttempt = _context.ExamAttempts.FindAsync(id);
            return Ok(examAttempt);

        }

        [HttpPost("FinishExamAttempt{examAttemptId}")]
        public async Task<IActionResult> FinishExamAttempt(int examAttemptId) {

            var score = await _examService.CalculateScore(examAttemptId);

            return Ok(score);
        }







        
    }
}

