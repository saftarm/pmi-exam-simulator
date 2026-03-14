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
using Microsoft.Build.Utilities;
namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

    

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // GET: api/Exam/Summary
        [HttpGet("/api/Summary")]
        public async Task<ActionResult<IEnumerable<ExamSummaryDto>>> GetSummaryAsync()
        {
            var examsSummary = await _examService.GetSummaryAsync();
            
            return Ok(examsSummary);
        }

        

        [HttpGet("Details/{id:int}")]

        public async Task<ActionResult<ExamDetailsDto>> GetDetailsByIdAsync(int id)
        {
            var examDetails = await _examService.GetDetailsByIdAsync(id);

            if(examDetails == null)
            {
                return NotFound($"Exam with Id: {id} not found");
            }
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


        
    }
}

