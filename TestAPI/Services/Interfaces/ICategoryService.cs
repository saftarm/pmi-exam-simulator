using TestAPI.DTO;
using TestAPI.DTO.Category;

namespace TestAPI.Services.Interfaces
{
    public interface ICategoryService
    {

        public Task<CategoryDto> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken ct);

        public Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct);

        public Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct);
        public Task UpdateCategory(Guid id, UpdateCategoryRequest dto);

        public Task DeleteAsync(Guid id);


    }
}
