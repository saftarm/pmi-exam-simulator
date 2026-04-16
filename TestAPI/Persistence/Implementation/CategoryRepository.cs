using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return true;
        }

        // Exists By Title
        public async Task<bool> ExistsByTitleAsync(string title, CancellationToken ct)
        {
            return await _context.Categories.AnyAsync(c => c.Title == title, ct);
        }


        // Get By Id 
        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            var category = await _context.Categories
            .Include(c => c.Exams)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<string> GetTitleByExamId(Guid examId)
        {

            var exam = await _context.Exams.FindAsync(examId);

            if (exam == null)
            {
                throw new Exception("Exam Not Found");
            }
            var category = await _context.Categories.FindAsync(exam.CategoryId);

            if (category == null || category.Title == null)
            {
                throw new Exception("Category Not Found");
            }

            return category.Title;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if(category == null) {
                throw new RecordNotFoundException("Category not found by Id");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category updatedCategory)
        {
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByCategoryId(Guid categoryId)
        {

            var exams = await _context.Exams.Where(e => e.CategoryId == categoryId).ToListAsync();

            if (exams == null)
            {
                throw new Exception("Category has no exams");
            }

            return exams;


        }
        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct) {
            return await _context.Categories.AnyAsync(c => c.Id == id, ct);
        }

    }
}