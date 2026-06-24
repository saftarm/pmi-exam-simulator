using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TestAPI.Models.Session;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

public sealed class RedisSessionSnapshotStore : ISessionSnapshotStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly IConnectionMultiplexer _multiplexer;
    private readonly RedisSettings _settings;
    private readonly ILogger<RedisSessionSnapshotStore> _logger;

    public RedisSessionSnapshotStore(
        IConnectionMultiplexer multiplexer,
        IOptions<RedisSettings> settings,
        ILogger<RedisSessionSnapshotStore> logger)
    {
        _multiplexer = multiplexer;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SaveAsync(SessionSnapshot snapshot, TimeSpan ttl, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        if (ttl <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(ttl), "TTL must be greater than zero.");
        }

        var db = _multiplexer.GetDatabase();
        var key = BuildKey(snapshot.SessionId);
        var payload = JsonSerializer.Serialize(snapshot, JsonOptions);

        var saved = await db.StringSetAsync(key, payload, ttl).ConfigureAwait(false);
        if (!saved)
        {
            _logger.LogError("Redis refused to save session snapshot for {SessionId}", snapshot.SessionId);
            throw new InvalidOperationException("Failed to save session snapshot to Redis.");
        }
    }

    public async Task<SessionSnapshot?> GetAsync(Guid sessionId, CancellationToken ct = default)
    {
        var db = _multiplexer.GetDatabase();
        var key = BuildKey(sessionId);
        var value = await db.StringGetAsync(key).ConfigureAwait(false);

        if (value.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<SessionSnapshot>(value.ToString(), JsonOptions);
    }

    public async Task DeleteAsync(Guid sessionId, CancellationToken ct = default)
    {
        var db = _multiplexer.GetDatabase();
        await db.KeyDeleteAsync(BuildKey(sessionId)).ConfigureAwait(false);
    }

    private string BuildKey(Guid sessionId) => $"{_settings.KeyPrefix}{sessionId}";
}
