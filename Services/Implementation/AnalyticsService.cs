using TestAPI.DTO.Analytics;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class AnalyticsService(IExamAttemptRepository examAttemptRepository) : IAnalyticsService
{
    private const int PassThresholdPercent = 80;

    public async Task<Result<IReadOnlyList<AttemptVolumeDto>>> GetAttemptVolumeAsync(int days, CancellationToken ct = default)
    {
        var rangeDays = days > 0 ? Math.Min(days, 365) : 30;
        var fromDate = DateTime.UtcNow.Date.AddDays(-rangeDays + 1);

        var byDate = await examAttemptRepository.GetCompletedAttemptVolumeByDayAsync(fromDate, ct);
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
        var completed = await examAttemptRepository.GetCompletedAttemptScoresAsync(ct);

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
