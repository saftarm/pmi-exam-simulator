namespace TestAPI.DTO.Public;

public record PublicStatsDto
{
    public int TotalQuestions { get; init; }
    public int TotalUsers { get; init; }
    public int PublishedExamCount { get; init; }
}
