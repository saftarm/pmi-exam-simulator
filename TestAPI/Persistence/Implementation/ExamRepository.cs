using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Exceptions;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;


namespace TestAPI.Persistence.Implementation
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam>?> GetPublishedExamsByCategoryIdAsync(Guid categoryId, PageParameters pageParameters, CancellationToken ct)
        {
            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.CategoryId == categoryId && e.Status == ExamStatus.Published)
                .OrderBy(e => e.CreatedAt)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToListAsync(ct);
        }

        public IQueryable<Exam> GetAllAsync()
        {
            var examsQuery = _context.Exams
            .Include(e => e.Category)
            .Include(e => e.Questions)
            .AsQueryable();
            return examsQuery;
        }

        public async Task AddAsync(IEnumerable<Exam> exams)
        {
            await _context.Exams.AddRangeAsync(exams);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId)
        {

            var questions = await _context.Questions
                .AsNoTracking()
                .Where(q => q.ExamId == examId)
                .Include(q => q.AnswerOptions)
                .ToListAsync();

            return questions;
        }

        public async Task UpdateAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(Guid examId)
        {
            await _context.Exams.Where(e => e.Id == examId).ExecuteDeleteAsync();


            await _context.SaveChangesAsync();

        }


        public async Task DeleteRangeAsync(IEnumerable<Guid> examIds)
        {
            await _context.Exams.Where(q => examIds.Contains(q.Id)).ExecuteDeleteAsync();

        }

        public async Task<Exam> GetByIdAsync(Guid id)
        {
            var examFromDb = await _context.Exams
            .Include(e => e.Domains)
            .Include(e => e.Category)
            .Include(e => e.Questions)
            .ThenInclude(q => q.AnswerOptions)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (examFromDb == null)
            {
                throw new RecordNotFoundException("Exam not found");
            }

            return examFromDb;
        }

        

        public async Task<IEnumerable<Guid>> GetDomainIdsById(Guid id)
        {
            var domainIds = await _context.Domains.Where(d => d.ExamId == id).Select(d => d.Id).ToListAsync();
            return domainIds;
        }


        public async Task<IEnumerable<Exam>> GetAllById(ICollection<Guid> examIds)
        {
            return await _context.Exams.Where(e => examIds.Contains(e.Id)).ToListAsync();

        }



        public async Task AddQuestionsToExamAsync(Guid examId, ICollection<Question> questions)
        {
            var exam = await _context.Exams.FindAsync(examId);

            if (exam == null)
            {
                throw new RecordNotFoundException("Exam not found");
            }

            foreach (var question in questions)
            {
                exam.Questions.Add(question);
            }

        }


        public async Task<ExamStatus> GetExamStatusByIdAsync(Guid id)
        {
            var examStatus = await _context.Exams
            .Where(e => e.Id == id)
            .Select(e => e.Status)
            .FirstOrDefaultAsync();

            return examStatus;

        }
    }
}
