namespace TestAPI.DTO.Analytics;

public record AttemptVolumeDto
{
    public DateOnly Date { get; init; }
    public int Count { get; init; }
}
