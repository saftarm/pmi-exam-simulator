using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Exceptions;
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

        // Get a single question by ID
        [HttpGet("/api/questions/{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById([FromQuery] Guid id)
        {
            var question = await _questionService.GetByIdAsync(id);
            return question;
        }

        // /api/QuestionGet all questions (hides isCorrect)
        [HttpGet("/api/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
            var questions = await _questionService.GetAllAsync();

            if (questions == null)
            {
                throw new RecordNotFoundException("Questions not found");
            }

            return Ok(questions);
        }

        [HttpPost("/api/questions/")]

        public async Task<IActionResult> CreateRange([FromForm] IFormFile file, CancellationToken ct)
        {
            var result = await _questionImportService.ImportFromExcelAsync(file, ct);

            return Ok(result);
        }

        // /api/Question/{id}  Update an existing question

        [HttpPut("/api/questions/{id:Guid}")]

        public async Task<ActionResult> Update(Guid id, UpdateQuestionDto updateQuestionDto)
        {
            var updatedQuestion = await _questionService.UpdateAsync(id, updateQuestionDto);
            return Ok(updatedQuestion);
        }

        // /api/Question/{id}	Delete a question
        [HttpDelete("/api/questions/{id}")]
        public IActionResult Delete(Guid id)
        {
            var question = _context.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }
            _context.Questions.Remove(question);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("/api/questions")]

        public async Task<IActionResult> DeleteRange(DeleteQuestionsRequest request)
        {

            if (request.QuestionIds == null)
            {
                throw new ArgumentNullException("Invalid Question Id input");
            }
            await _questionService.DeleteRangeAsync(request.QuestionIds);
            return Ok();
        }

    }
}
