
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IQuestionService _questionService;

        public QuestionController(ApplicationDbContext context, IQuestionService questionService)
        {
            _context = context;
            _questionService = questionService;

        }

        
        // /api/Question/{id}	Get a single question by ID
        [HttpGet("/api/Question/{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id) {
            return await _questionService.GetByIdAsync(id);
        }


        // /api/Question	Get all questions (hides isCorrect)
        [HttpGet("/api/Question")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {

            var questions = await _questionService.GetAllAsync();

            if(questions == null ) {
                throw new Exception("Questions not found");
            }

            return Ok(questions);

        }

        // /api/Question	Create a new question with answer options
        [HttpPost]
        public async Task<CreatedAtActionResult> Create(CreateQuestionDto createQuestionDto){
            var newQuestionid = await _questionService.CreateAsync(createQuestionDto);
            return CreatedAtAction(nameof(GetQuestionById), new { id = newQuestionid}, createQuestionDto);
        }


        // /api/Question/{id}	Update an existing question

        [HttpPut("/Update/{id}")]

        public async Task<ActionResult> Update(int id, UpdateQuestionDto updateQuestionDto){
            var updatedQuestion = await _questionService.UpdateAsync(id,updateQuestionDto);
            return Ok(updatedQuestion);
        }


        // /api/Question/{id}	Delete a question
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
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



     


    }
}
