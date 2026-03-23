using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
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

        public async Task<Category>? GetByIdAsync(int categoryId)
        {
            var category = await _context.Categories
            .Include(c => c.Exams)
            .FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category Not Found");
            }
            return category;

        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();

        }

        public async Task<string> GetTitleByExamId(int examId)
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

        public async Task DeleteAsync(int categoryId)
        {

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category updatedCategory)
        {
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByCategoryId(int categoryId)
        {

            var exams = await _context.Exams.Where(e => e.CategoryId == categoryId).ToListAsync();

            if (exams == null)
            {
                throw new Exception("Category has no exams");
            }

            return exams;


        }



        //public async Task<IEnumerable<ExamDto>> GetExams { get; set; }









    }
}