using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Services.Interfaces;
namespace TestAPI.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;



        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet("/api/categories/{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("/api/categories")]

        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        // [HttpPost("/AddExam")]

        // public async Task<IActionResult> AddExamsToCategory(AddExamsToCategoryDto addExamsToCategoryDto) {
        //     await  _categoryService.AddExamsToCategory(addExamsToCategoryDto);
        //     return Ok();
        // }

        [HttpPost("api/categories")]

        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
        {
            await _categoryService.CreateCategory(dto);
            return Ok();
        }

        [HttpPatch("api/categories/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto) {
            await _categoryService.UpdateCategory(id, dto);
            return Ok();
        }

        [HttpDelete("/api/categories/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
        



       








    }
}

