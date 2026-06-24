using TestAPI.Models.Session;

namespace TestAPI.Persistence.Interfaces;

public interface ISessionSnapshotStore
{
    Task SaveAsync(SessionSnapshot snapshot, TimeSpan ttl, CancellationToken ct = default);

    Task<SessionSnapshot?> GetAsync(Guid sessionId, CancellationToken ct = default);

    Task DeleteAsync(Guid sessionId, CancellationToken ct = default);
}
