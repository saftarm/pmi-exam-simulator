namespace TestAPI.DTO.Exam;

public record ExamOverviewStatsDto
{
    public Guid ExamId { get; init; }
    public string ExamTitle { get; init; } = string.Empty;
    public int AttemptCount { get; init; }
    public int UniqueUsersCount { get; init; }
    public decimal AverageScore { get; init; }
}
