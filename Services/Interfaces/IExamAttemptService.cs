using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IExamAttemptService
    {
        Task<Result<SessionDto>> StartSession(Guid userId, Guid examId, CancellationToken ct = default);
        Task<Result<SessionResultDto>> FinishSession(
            FinishSessionRequest request,
            Guid actingUserId,
            CancellationToken ct);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> AbandonSessionAsync(Guid sessionId, Guid actingUserId, CancellationToken ct = default);
    }
}
