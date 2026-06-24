using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces;

public interface IDomainPerformanceRepository
{
    Task<IReadOnlyList<DomainPerformance>> GetByUserIdWithDetailsAsync(Guid userId, CancellationToken ct = default);
    Task UpsertSessionStatsAsync(
        Guid userId,
        Guid examId,
        IReadOnlyDictionary<Guid, (decimal ScorePoints, int QuestionCount)> statsByDomain,
        CancellationToken ct = default);
}
