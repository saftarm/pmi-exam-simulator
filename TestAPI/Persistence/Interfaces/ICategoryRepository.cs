using DocumentFormat.OpenXml.Office2010.Excel;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category?> GetByIdAsync(Guid categoryId);
        public Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct);
        public Task<string?> GetTitleByExamId(Guid examId, CancellationToken ct);
        public Task AddAsync(Category category);
        public Task Update(Category Category);
        public Task DeleteAsync(Guid categoryId);
        public Task<IEnumerable<Exam>> GetExamsByCategoryId(Guid category);
        public Task<bool> ExistsByTitleAsync(string title, CancellationToken ct);
        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
 

    }
}