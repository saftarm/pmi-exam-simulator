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
        IReadOnlyDictionary<Guid, (int CorrectCount, int TotalCount)> statsByDomain,
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

        foreach (var stat in statsByDomain)
        {
            var existing = existingRecords.FirstOrDefault(r => r.DomainId == stat.Key);

            if (existing == null)
            {
                await _context.DomainPerformances.AddAsync(new DomainPerformance
                {
                    UserId = userId,
                    ExamId = examId,
                    DomainId = stat.Key,
                    TotalAnswered = stat.Value.TotalCount,
                    TotalCorrect = stat.Value.CorrectCount,
                    LastUpdated = now,
                }, ct);
            }
            else
            {
                existing.TotalCorrect += stat.Value.CorrectCount;
                existing.TotalAnswered += stat.Value.TotalCount;
                existing.LastUpdated = now;
            }
        }
    }
}
