using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Question;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
    public class QuestionRepository(
        ApplicationDbContext context,
        IDbContextFactory<ApplicationDbContext> dbFactoryContext) : IQuestionRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactoryContext = dbFactoryContext;

        public async Task<IEnumerable<Question>> QueryQuestionsWithAnswerOptions(
            Dictionary<Guid, int> numberOfQuestionsPerDomain)
        {
            if (numberOfQuestionsPerDomain.Count == 0)
            {
                return [];
            }

            var queryTasks = numberOfQuestionsPerDomain.Select(async kvp =>
            {
                await using var context = await _dbFactoryContext.CreateDbContextAsync();
                return await context.Questions
              .AsNoTracking()
              .Where(q => q.DomainId == kvp.Key)
              .Include(q => q.AnswerOptions)
              .OrderBy(q => EF.Functions.Random())
              .Take(kvp.Value)
              .ToListAsync();
            });

            var results = await Task.WhenAll(queryTasks);
            return results.SelectMany(questions => questions);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        {
            return await _context.Questions.AnyAsync(q => q.Id == id, ct);
        }

        public async Task AddAsync(Question newQuestion)
        {
            await _context.Questions.AddAsync(newQuestion);
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> questionIds)
        {
            return await _context.Questions
              .Where(q => questionIds.Contains(q.Id))
              .ExecuteDeleteAsync();
        }

        public Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            return Task.CompletedTask;
        }

        public Task AddRangeAsync(IEnumerable<Question> questions)
        {
            _context.Questions.AddRange(questions);
            return Task.CompletedTask;
        }

        public async Task DeleteQuestionById(Guid questionId)
        {
            await _context.Questions
              .Where(q => q.Id == questionId)
              .ExecuteDeleteAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid questionId)
        {
            return await _context.Questions.FindAsync(questionId);
        }

        public async Task<Question?> GetByIdWithOptionsAsync(Guid questionId, CancellationToken ct)
        {
            return await _context.Questions
                .Include(q => q.AnswerOptions)
                .Include(q => q.Domain)
                .ThenInclude(d => d!.Exam)
                .FirstOrDefaultAsync(q => q.Id == questionId, ct);
        }

        public async Task<PagedList<QuestionListItemDto>> GetPagedAsync(
            QuestionQueryParameters query,
            CancellationToken ct)
        {
            var page = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 20;

            var questionsQuery = _context.Questions.AsNoTracking().AsQueryable();

            if (query.DomainId.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.DomainId == query.DomainId.Value);
            }

            if (query.QuestionType.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.QuestionType == query.QuestionType.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var term = query.Search.Trim();
                questionsQuery = questionsQuery.Where(q => q.Title != null && q.Title.Contains(term));
            }

            var projected = questionsQuery
                .OrderBy(q => q.CreatedAt)
                .Select(q => new QuestionListItemDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    QuestionType = q.QuestionType,
                    DomainId = q.DomainId,
                    DomainTitle = q.Domain != null ? q.Domain.Title : string.Empty,
                    ExamTitle = q.Domain != null && q.Domain.Exam != null ? q.Domain.Exam.Title : string.Empty,
                    AnswerOptionCount = q.AnswerOptions != null ? q.AnswerOptions.Count : 0
                });

            return await PagedList<QuestionListItemDto>.CreateAsync(projected, page, pageSize);
        }

        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsByIds(IEnumerable<Guid> optionsIds)
        {
            return await _context.AnswerOptions
              .Where(o => optionsIds.Contains(o.Id))
              .ToListAsync();
        }

        public async Task<IReadOnlyList<AnswerOption>> GetAnswerOptionsByQuestionIds(
            IEnumerable<Guid> questionIds,
            CancellationToken ct = default)
        {
            var ids = questionIds.Distinct().ToList();
            if (ids.Count == 0)
            {
                return [];
            }

            return await _context.AnswerOptions
                .AsNoTracking()
                .Where(o => ids.Contains(o.QuestionId))
                .ToListAsync(ct);
        }
    }
}
