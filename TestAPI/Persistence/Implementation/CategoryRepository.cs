





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

            var category = await _context.Categories.FindAsync(categoryId);


            if (category == null)
            {
                throw new Exception("Category Not Found");
            }


            return category;

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

            await _context.Categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task Update(int categoryId, Category updatedCategory)
        {
            _context.Categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExamSummaryDto>> GetExamSummariesByCategoryId(int categoryId)
        {
            var category = await _context.Categories
            .Include(c => c.Exams)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new Exception("Category Not Found");
            }

            if (category.Exams == null)
            {
                throw new Exception("Exams Not Found");
            }

            var examSummaryDtos = category.Exams.Select(
                e => new ExamSummaryDto
                {
                    Id = e.Id,
                    CategoryId = e.CategoryId,
                    Title = e.Title,
                    DurationInMinutes = e.DurationInMinutes,
                    NumberOfQuestions = e.NumberOfQuestions
                }
            );

            return examSummaryDtos;


        }

        //public async Task<IEnumerable<ExamDto>> GetExams { get; set; }








    }
}