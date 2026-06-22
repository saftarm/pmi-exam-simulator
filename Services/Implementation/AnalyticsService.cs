using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTO.Analytics;
using TestAPI.Enums;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class AnalyticsService(ApplicationDbContext context) : IAnalyticsService
{
    private const int PassThresholdPercent = 80;

    private readonly ApplicationDbContext _context = context;

    public async Task<Result<IReadOnlyList<AttemptVolumeDto>>> GetAttemptVolumeAsync(int days, CancellationToken ct = default)
    {
        var rangeDays = days > 0 ? Math.Min(days, 365) : 30;
        var fromDate = DateTime.UtcNow.Date.AddDays(-rangeDays + 1);

        var grouped = await _context.ExamAttempts
            .AsNoTracking()
            .Where(a => a.Status == AttemptStatus.Completed && a.SubmittedAt >= fromDate)
            .GroupBy(a => a.SubmittedAt!.Value.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var byDate = grouped.ToDictionary(x => DateOnly.FromDateTime(x.Date), x => x.Count);
        var result = new List<AttemptVolumeDto>();
        for (var i = 0; i < rangeDays; i++)
        {
            var date = DateOnly.FromDateTime(fromDate.AddDays(i));
            result.Add(new AttemptVolumeDto
            {
                Date = date,
                Count = byDate.GetValueOrDefault(date),
            });
        }

        return Result<IReadOnlyList<AttemptVolumeDto>>.Success(result);
    }

    public async Task<Result<PassRateAnalyticsDto>> GetPassRateAsync(CancellationToken ct = default)
    {
        var completed = await _context.ExamAttempts
            .AsNoTracking()
            .Where(a => a.Status == AttemptStatus.Completed)
            .Select(a => a.PercentageScore)
            .ToListAsync(ct);

        var total = completed.Count;
        var passCount = completed.Count(s => s >= PassThresholdPercent);
        var average = total > 0 ? completed.Average(s => s) : 0m;
        var passRate = total > 0 ? (decimal)passCount / total * 100m : 0m;

        return Result<PassRateAnalyticsDto>.Success(new PassRateAnalyticsDto
        {
            AverageScore = Math.Round(average, 1),
            TotalCompletedAttempts = total,
            PassCount = passCount,
            PassRate = Math.Round(passRate, 1),
            PassThreshold = PassThresholdPercent,
        });
    }
}
