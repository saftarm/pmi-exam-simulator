using TestAPI.DTO;
using TestAPI.DTO.Category;

namespace TestAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDto> GetByIdAsync(int categoryId);

        public Task<string> GetTitleByExamId(int examId);

        public Task CreateCategory(CreateCategoryDto createCategoryDto);

        public Task AddExamToCategory(AddExamsToCategoryDto addExamsToCategoryDto);







    }
}
