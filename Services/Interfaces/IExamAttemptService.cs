using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
  public interface IExamAttemptService
  {
    Task<Result<SessionDto>> StartSession(Guid userId, Guid examId);
    Task<Result<SessionResultDto>> FinishSession(FinishSessionRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id);
  }
}
