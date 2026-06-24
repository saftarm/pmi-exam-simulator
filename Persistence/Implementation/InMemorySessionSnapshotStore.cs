using System.Collections.Concurrent;
using TestAPI.Models.Session;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

/// <summary>
/// Development fallback when Redis is not configured. Not suitable for multi-instance deployments.
/// </summary>
public sealed class InMemorySessionSnapshotStore : ISessionSnapshotStore
{
    private readonly ConcurrentDictionary<Guid, CacheEntry> _entries = new();

    public Task SaveAsync(SessionSnapshot snapshot, TimeSpan ttl, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (ttl <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(ttl), "TTL must be greater than zero.");
        }

        _entries[snapshot.SessionId] = new CacheEntry(snapshot, DateTime.UtcNow.Add(ttl));
        return Task.CompletedTask;
    }

    public Task<SessionSnapshot?> GetAsync(Guid sessionId, CancellationToken ct = default)
    {
        if (!_entries.TryGetValue(sessionId, out var entry))
        {
            return Task.FromResult<SessionSnapshot?>(null);
        }

        if (entry.ExpiresAt <= DateTime.UtcNow)
        {
            _entries.TryRemove(sessionId, out _);
            return Task.FromResult<SessionSnapshot?>(null);
        }

        return Task.FromResult<SessionSnapshot?>(entry.Snapshot);
    }

    public Task DeleteAsync(Guid sessionId, CancellationToken ct = default)
    {
        _entries.TryRemove(sessionId, out _);
        return Task.CompletedTask;
    }

    private sealed record CacheEntry(SessionSnapshot Snapshot, DateTime ExpiresAt);
}
