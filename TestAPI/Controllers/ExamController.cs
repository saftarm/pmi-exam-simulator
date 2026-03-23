using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;
using TestAPI.DTO;
using TestAPI.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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

        public async Task<IActionResult> GetPublishedExams([FromQuery] PageParameters pageParameters) {
            var exams = await _examService.GetPublishedExamsAsync(pageParameters);

            return Ok(exams);
        }

        
        // [HttpPost("/api/exams")]
        // public async Task<IActionResult> Create(CreateExamsDto dto) {
        //     await _examService.CreateExam(dto);
        //     return Ok();
        // }

        [HttpPost("/api/exams")]
        public async Task<IActionResult> Create([FromBody] List<CreateExamDto> dto) {
            await _examService.CreateExams(dto);
            return Ok();
        }
        // [HttpPost("/api/exam/bulk")]

        // public async Task<IActionResult> CreateBulk(CreateExamDto createExamDto) {
        //     await _examService.CreateExam(createExamDto);
        //     return Ok();
        // }

        // // GET: api/Exam/Summary
        [HttpGet("/api/exams/summary")]
        public async Task<ActionResult<IEnumerable<ExamSummaryDto>>> GetSummariesAsync([FromQuery] PageParameters pageParameters)
        {
            var examsSummaries = await _examService.GetSummariesAsync(pageParameters);
            
            return Ok(examsSummaries);
        }
        [HttpGet("/api/exams/{id:int}/summary")]
        public async Task<ActionResult<ExamSummaryDto>> GetSummaryByIdAsync(int id)
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

        public async Task<ActionResult<ExamDetailsDto>> GetDetailsByIdAsync(int id)
        {

            var examDetails = await _examService.GetDetailsByIdAsync(id);

            if(examDetails == null)
            {
                return NotFound($"Exam with Id: {id} not found");
            }
            return Ok(examDetails);
        }






        [HttpDelete("api/exams/{id:int}")]

        public async Task<IActionResult> Delete(int id){
            await _examService.DeleteAsync(id);

            return NoContent();
        }
        [HttpDelete("api/exams")]
        public async Task<IActionResult> DeleteRange(DeleteExamsRequest request) {

            if(request.ExamIds == null) {
                throw new ArgumentNullException("Invalid Exam Id input");
            }

            await _examService.DeleteRangeAsync(request.ExamIds);
            return NoContent();
        }



        
    //     [HttpPost("/api/exams/{id:int}")]
    //     public async Task<ActionResult<Exam>> CompileExam(int id)
    //     {
    //        await _examService.CompileExam(id);
    //        return Ok();
    // }



    }
}

