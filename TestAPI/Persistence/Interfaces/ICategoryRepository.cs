using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category>? GetByIdAsync(int categoryId);

        public Task<IEnumerable<Category>> GetAllAsync();

        //public Task<string> GetTitleByExamId(int examId);

        public Task AddAsync(Category category);
        public Task Update(Category Category);

        public Task DeleteAsync(int categoryId); 

        public Task<IEnumerable<Exam>> GetExamsByCategoryId(int category);


        //public Task AddExamsToCategory(Category category);


    }
}