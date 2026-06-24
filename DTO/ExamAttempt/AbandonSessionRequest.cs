namespace TestAPI.DTO.ExamAttempt;

public record AbandonSessionRequest
{
    public Guid SessionId { get; init; }
}
