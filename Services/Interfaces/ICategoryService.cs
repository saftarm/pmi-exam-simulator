using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
  public interface ICategoryService
  {
    public Task<Result> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken ct);
    public Task<Result<CategoryDto>> GetByIdAsync(Guid categoryId, CancellationToken ct);
    public Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken ct);
    public Task<Result> UpdateCategory(UpdateCategoryRequest dto, CancellationToken ct);
    public Task<Result> DeleteAsync(Guid id, CancellationToken ct);
  }
}
