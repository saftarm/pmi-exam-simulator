namespace TestAPI.DTO.Analytics;

public record PassRateAnalyticsDto
{
    public decimal AverageScore { get; init; }
    public int TotalCompletedAttempts { get; init; }
    public int PassCount { get; init; }
    public decimal PassRate { get; init; }
    public int PassThreshold { get; init; }
}
