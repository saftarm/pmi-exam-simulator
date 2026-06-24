using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Exam;
using TestAPI.Entities;
using TestAPI.Enums;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation
{
    public class ExamAttemptRepository : IExamAttemptRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamAttemptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ExamAttempt examAttempt)
        {
            await _context.ExamAttempts.AddAsync(examAttempt);
        }

        public async Task SaveSessionResponses(IEnumerable<UserExamResponse> responses)
        {
            await _context.UserExamResponses.AddRangeAsync(responses);
        }

        public Task UpdateAsync(ExamAttempt updatedExamAttempt)
        {
            _context.ExamAttempts.Update(updatedExamAttempt);
            return Task.CompletedTask;
        }

        public async Task<ExamAttempt?> GetByIdForFinishAsync(Guid examAttemptId, CancellationToken ct)
        {
            return await _context.ExamAttempts
                .FirstOrDefaultAsync(ea => ea.Id == examAttemptId, ct);
        }

        public async Task<ExamAttempt?> GetByIdAsync(Guid examAttemptId)
        {
            return await _context.ExamAttempts
                .Include(ea => ea.UserExamResponses)
                .FirstOrDefaultAsync(ea => ea.Id == examAttemptId);
        }

        public async Task<IEnumerable<ExamAttempt>> GetAllAsync()
        {
            return await _context.ExamAttempts.ToListAsync();
        }

        public async Task<ExamAttempt?> GetByUserId(Guid userId)
        {
            return await _context.ExamAttempts
                .Include(a => a.UserExamResponses)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task DeleteAsync(Guid examAttemptId)
        {
            await _context.ExamAttempts
                .Where(a => a.Id == examAttemptId)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<UserExamResponse>> GetResponsesAsync(Guid examAttemptId)
        {
            return await _context.UserExamResponses
                .Where(r => r.ExamAttemptId == examAttemptId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ExamAttempt>> GetAttemptsByExamAndUserIdAsync(
            Guid userId,
            Guid examId,
            CancellationToken ct)
        {
            return await _context.ExamAttempts
                .Where(a => a.UserId == userId && a.ExamId == examId)
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.ExamAttempts.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> HasActiveSessionAsync(Guid userId, Guid examId, CancellationToken ct = default)
        {
            return await _context.ExamAttempts.AnyAsync(
                a => a.UserId == userId
                     && a.ExamId == examId
                     && a.Status == AttemptStatus.InProgress,
                ct);
        }

        public async Task<IReadOnlyList<ExamOverviewStatsDto>> GetOverviewStatsAsync(CancellationToken ct = default)
        {
            var completedAttempts = _context.ExamAttempts
                .AsNoTracking()
                .Where(a => a.Status == AttemptStatus.Completed);

            var stats = await (
                from exam in _context.Exams.AsNoTracking()
                join attempt in completedAttempts on exam.Id equals attempt.ExamId into attempts
                select new ExamOverviewStatsDto
                {
                    ExamId = exam.Id,
                    ExamTitle = exam.Title,
                    AttemptCount = attempts.Count(),
                    UniqueUsersCount = attempts.Select(a => a.UserId).Distinct().Count(),
                    AverageScore = attempts.Any()
                      ? attempts.Average(a => a.PercentageScore)
                      : 0m,
                })
                .OrderByDescending(s => s.AttemptCount)
                .ThenBy(s => s.ExamTitle)
                .ToListAsync(ct);

            return stats;
        }

        public async Task<Dictionary<Guid, int>> GetCompletedAttemptCountsByExamAsync(CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .AsNoTracking()
                .Where(a => a.Status == AttemptStatus.Completed)
                .GroupBy(a => a.ExamId)
                .Select(g => new { ExamId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ExamId, x => x.Count, ct);
        }

        public async Task<IReadOnlyDictionary<DateOnly, int>> GetCompletedAttemptVolumeByDayAsync(
            DateTime fromDate,
            CancellationToken ct = default)
        {
            var grouped = await _context.ExamAttempts
                .AsNoTracking()
                .Where(a => a.Status == AttemptStatus.Completed && a.SubmittedAt >= fromDate)
                .GroupBy(a => a.SubmittedAt!.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            return grouped.ToDictionary(
                x => DateOnly.FromDateTime(x.Date),
                x => x.Count);
        }

        public async Task<IReadOnlyList<decimal>> GetCompletedAttemptScoresAsync(CancellationToken ct = default)
        {
            return await _context.ExamAttempts
                .AsNoTracking()
                .Where(a => a.Status == AttemptStatus.Completed)
                .Select(a => a.PercentageScore)
                .ToListAsync(ct);
        }
    }
}
