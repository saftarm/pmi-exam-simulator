using TestAPI.DTO.Analytics;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces;

public interface IAnalyticsService
{
    Task<Result<IReadOnlyList<AttemptVolumeDto>>> GetAttemptVolumeAsync(int days, CancellationToken ct = default);
    Task<Result<PassRateAnalyticsDto>> GetPassRateAsync(CancellationToken ct = default);
}
