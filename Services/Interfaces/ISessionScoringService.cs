using TestAPI.DTO;
using TestAPI.DTO.ExamAttempt;
using TestAPI.Models.Session;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces;

public interface ISessionScoringService
{
    Task<Result<SessionCalculationResult>> ValidateAndScoreAsync(
        SessionSnapshot snapshot,
        Guid sessionId,
        IEnumerable<UserExamResponseDto> responses,
        CancellationToken ct = default);
}
