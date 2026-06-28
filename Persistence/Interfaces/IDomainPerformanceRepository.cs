using TestAPI.DTO;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces;

public interface IDomainPerformanceRepository
{
    Task<IReadOnlyList<DomainPerformance>> GetByUserIdWithDetailsAsync(Guid userId, CancellationToken ct = default);
    Task UpsertSessionStatsAsync(
        Guid userId,
        Guid examId,
        IReadOnlyDictionary<Guid, (int CorrectCount, int TotalCount)> statsByDomain,
        CancellationToken ct = default);
}
