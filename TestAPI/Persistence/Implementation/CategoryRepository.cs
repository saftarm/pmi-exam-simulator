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

        // Get all categories
        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Categories.ToListAsync();
        }
        
        // Get Category title by examId
        public async Task<string?> GetTitleByExamId(Guid examId, CancellationToken ct)
        {
            var title = await _context.Exams
            .Where(e => e.Id == examId)
            .Select(e => e.Category.Title)
            .FirstOrDefaultAsync(ct);

            return title;
        }
        // Adding new category
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        // Hard delete by CategoryId
        public async Task DeleteAsync(Guid categoryId)
        {
            await _context.Categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync();
        }

        // Updating category
        public async Task Update(Category updatedCategory)
        {
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
        }

        // Get all exams by CategoryId
        public async Task<IEnumerable<Exam>> GetExamsByCategoryId(Guid categoryId)
        {
            return await _context.Exams.Where(e => e.CategoryId == categoryId).ToListAsync();
        }

        // Check if Category exists by its ID
        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct) {
            return await _context.Categories.AnyAsync(c => c.Id == id, ct);
        }

    }
}