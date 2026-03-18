using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Services.Interfaces;
namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;



        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpPost("/AddExam")]

        public async Task<IActionResult> AddExamsToCategory(AddExamsToCategoryDto addExamsToCategoryDto) {
            await  _categoryService.AddExamsToCategory(addExamsToCategoryDto);
            return Ok();
        }

        [HttpPost]

        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto createCategoryDto)
        {
            await _categoryService.CreateCategory(createCategoryDto);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }



       








    }
}

