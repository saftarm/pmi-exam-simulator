using StackExchange.Redis;
using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.Entities;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
  public interface IExamAttemptService
  {
    public Task<Result<SessionDto>> StartSession(Guid userId, Guid examId);

    public Task<Result<SessionResultDto>> FinishSession(FinishSessionRequest request, CancellationToken ct);


    public Task DeleteAsync(Guid id);
  }
}
