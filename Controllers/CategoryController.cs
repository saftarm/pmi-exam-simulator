using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Extensions;
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
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("api/categories")]
    public async Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken ct)
    {
      var result = await _categoryService.CreateCategory(dto, ct);
      return result.IsSuccess ? Created() : result.ToActionResult();
    }

    // Get Category
    [HttpGet("/api/categories/{id}")]
    public async Task<IActionResult> GetCategory(Guid id, CancellationToken ct)
    {
      var result = await _categoryService.GetByIdAsync(id, ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    // Get All Categories
    [HttpGet("/api/categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
      var result = await _categoryService.GetAllAsync(ct);
      return result.IsSuccess ? Ok(result.Value) : result.ToActionResult();
    }

    // Update Category
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("api/categories/{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
      var result = await _categoryService.UpdateCategory(request, ct);
      return result.IsSuccess ? NoContent() : result.ToActionResult();
    }

    // Delete Category
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("/api/categories/{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
      var result = await _categoryService.DeleteAsync(id, ct);
      return result.IsSuccess ? NoContent() : result.ToActionResult();
    }

  }
}

