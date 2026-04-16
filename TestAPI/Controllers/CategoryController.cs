using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
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
        // Create Category
        [Authorize]
        [HttpPost("api/categories")]
        public async Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken ct)
        {
            var newCategory = await _categoryService.CreateCategory(dto, ct);
            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Id }, newCategory);
        }
        // Get Category
        [HttpGet("/api/categories/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id, CancellationToken ct)
        {
            var category = await _categoryService.GetByIdAsync(id, ct);
            return Ok(category);
        }

        // Get All Categories
        [HttpGet("/api/categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(CancellationToken ct)
        {
            var categories = await _categoryService.GetAllAsync(ct);
            return Ok(categories);
        }

        // Update Category
        [HttpPut("api/categories/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            await _categoryService.UpdateCategory(id, request);
            return Ok();
        }

        // Delete Category
        [HttpDelete("/api/categories/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
        
    }
}

