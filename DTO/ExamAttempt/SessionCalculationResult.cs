using TestAPI.Entities;

namespace TestAPI.DTO.ExamAttempt
{
  public sealed class SessionCalculationResult
  {
    public required SessionResultDto Result { get; init; }
    public required IReadOnlyList<UserExamResponse> SavedResponses { get; init; }
  }
}
