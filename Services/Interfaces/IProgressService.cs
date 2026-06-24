using TestAPI.DTO.Progress;
using TestAPI.Entities;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces
{
    public interface IProgressService
    {
        Task<Result<IEnumerable<DomainPerformanceDto>>> GetUserDomainPerformancesAsync(Guid userId, CancellationToken ct);
        Task UpdateDomainPerformanceAsync(
            Guid userId,
            Guid examId,
            IReadOnlyDictionary<Guid, (decimal ScorePoints, int QuestionCount)> statsByDomain,
            CancellationToken ct);
    }
}
