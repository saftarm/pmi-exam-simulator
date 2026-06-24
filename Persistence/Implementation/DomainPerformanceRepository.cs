using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

public class DomainPerformanceRepository(ApplicationDbContext context) : IDomainPerformanceRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IReadOnlyList<DomainPerformance>> GetByUserIdWithDetailsAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        return await _context.DomainPerformances
            .AsNoTracking()
            .Include(dp => dp.Domain)
            .Include(dp => dp.Exam)
            .Where(dp => dp.UserId == userId)
            .OrderByDescending(dp => dp.LastUpdated)
            .ToListAsync(ct);
    }

    public async Task UpsertSessionStatsAsync(
        Guid userId,
        Guid examId,
        IReadOnlyDictionary<Guid, (decimal ScorePoints, int QuestionCount)> statsByDomain,
        CancellationToken ct = default)
    {
        if (statsByDomain.Count == 0)
        {
            return;
        }

        var existingRecords = await _context.DomainPerformances
            .Where(dp => dp.UserId == userId && dp.ExamId == examId)
            .ToListAsync(ct);

        var now = DateTime.UtcNow;

        foreach (var (domainId, stats) in statsByDomain)
        {
            var existing = existingRecords.FirstOrDefault(r => r.DomainId == domainId);

            if (existing == null)
            {
                await _context.DomainPerformances.AddAsync(new DomainPerformance
                {
                    UserId = userId,
                    ExamId = examId,
                    DomainId = domainId,
                    TotalAnswered = stats.QuestionCount,
                    TotalCorrect = stats.ScorePoints,
                    LastUpdated = now,
                }, ct);
            }
            else
            {
                existing.TotalCorrect += stats.ScorePoints;
                existing.TotalAnswered += stats.QuestionCount;
                existing.LastUpdated = now;
            }
        }
    }
}
