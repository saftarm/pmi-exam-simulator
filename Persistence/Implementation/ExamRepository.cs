using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Models.Pagination;
using TestAPI.Persistence.Interfaces;


namespace TestAPI.Persistence.Implementation
{
    public class ExamRepository(ApplicationDbContext context) : IExamRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<Exam?> QueryExamsWithDomainsById(Guid examId)
        {

            return await _context.Exams
              .AsNoTracking()
              .Where(e => e.Id == examId)
              .Include(e => e.Domains)
              .FirstOrDefaultAsync();

        }
        public async Task AddAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
        }

        public Task UpdateAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Exam>> GetPublishedPaginatedExamsAsync(PageParameters pageParameters)
        {
            var pageNumber = pageParameters.PageNumber > 0 ? pageParameters.PageNumber : 1;
            var pageSize = pageParameters.PageSize > 0 ? pageParameters.PageSize : 20;

            return await _context.Exams
              .Where(e => e.Status == ExamStatus.Published)
              .OrderBy(e => e.Title)
              .Skip(pageSize * (pageNumber - 1))
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetAllExams()
        {
            return await _context.Exams.ToListAsync();
        }

        public async Task<IReadOnlyList<ExamSummaryDto>> GetAllExamSummariesAsync(CancellationToken ct = default)
        {
            return await _context.Exams
                .AsNoTracking()
                .Include(e => e.Category)
                .OrderBy(e => e.Title)
                .Select(e => new ExamSummaryDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    CategoryId = e.CategoryId,
                    CategoryTitle = e.Category != null ? e.Category.Title : string.Empty,
                    NumberOfQuestions = e.NumberOfQuestions,
                    DurationInMinutes = e.DurationInMinutes,
                    Status = e.Status,
                })
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Exam>> GetPublishedExamsByCategoryIdAsync(Guid categoryId, PageParameters pageParameters, CancellationToken ct)
        {
            var pageNumber = pageParameters.PageNumber > 0 ? pageParameters.PageNumber : 1;
            var pageSize = pageParameters.PageSize > 0 ? pageParameters.PageSize : 20;

            return await _context.Exams
                .AsNoTracking()
                .Where(e => e.CategoryId == categoryId && e.Status == ExamStatus.Published)
                .OrderBy(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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


        // Get questions(and answer options) of specific exam
        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(Guid examId)
        {
            return await _context.Questions
                .AsNoTracking()
                .Where(q => q.ExamId == examId)
                .Include(q => q.AnswerOptions)
                .ToListAsync();
        }
        // Hard delet Exam by id
        public async Task DeleteAsync(Guid examId)
        {
            await _context.Exams.Where(e => e.Id == examId).ExecuteDeleteAsync();
        }

        // Hard delete multiple Exams by ids
        public async Task DeleteRangeAsync(IEnumerable<Guid> examIds)
        {
            await _context.Exams.Where(q => examIds.Contains(q.Id)).ExecuteDeleteAsync();

        }

        public async Task<Exam?> GetByIdAsync(Guid examId)
        {
            return await _context.Exams.FindAsync(examId);
        }

        public async Task<IEnumerable<Guid>> GetDomainIdsById(Guid id)
        {
            var domainIds = await _context.Domains.Where(d => d.ExamId == id).Select(d => d.Id).ToListAsync();
            return domainIds;
        }

        public async Task<IEnumerable<Exam>> GetAllByIds(ICollection<Guid> examIds)
        {
            return await _context.Exams.Where(e => examIds.Contains(e.Id)).ToListAsync();

        }
        public async Task<ExamStatus?> GetExamStatusByIdAsync(Guid id)
        {
            var examStatus = await _context.Exams
            .Where(e => e.Id == id)
            .Select(e => e.Status)
            .FirstOrDefaultAsync();

            return examStatus;

        }

        public async Task<int?> QueryNumberOfQuestionsByExamId(Guid examId)
        {
            return await _context.Exams
              .Where(e => e.Id == examId)
              .Select(e => (int?)e.NumberOfQuestions)
              .FirstOrDefaultAsync();
        }

        public async Task<bool> ExamExistsByExamId(Guid examId)
        {
            return await _context.Exams.AnyAsync(e => e.Id == examId);
        }

        public async Task<bool> ExamExistsByTitleAsync(string? title)
        {
            return await _context.Exams.AnyAsync(e => e.Title == title);
        }
    }
}
