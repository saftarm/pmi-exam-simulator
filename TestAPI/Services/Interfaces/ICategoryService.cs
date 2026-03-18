using TestAPI.DTO;
using TestAPI.DTO.Category;
using TestAPI.Entities;

namespace TestAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDto> GetByIdAsync(int categoryId);

        public Task<IEnumerable<CategoryDto>> GetAllAsync();


        public Task CreateCategory(CreateCategoryDto createCategoryDto);

        public Task DeleteAsync(int id);

        public Task AddExamsToCategory(AddExamsToCategoryDto addExamsToCategoryDto);

        public Task<IEnumerable<ExamSummaryDto>> GetExamSummariesByCategoryId(int categoryId);







    }
}
