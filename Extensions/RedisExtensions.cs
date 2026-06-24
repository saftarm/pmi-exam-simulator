using StackExchange.Redis;
using TestAPI.Models.Session;
using TestAPI.Persistence.Implementation;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Extensions;

public static class RedisExtensions
{
    public static IServiceCollection AddSessionSnapshotStore(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SectionName));

        var connectionString = configuration.GetSection(RedisSettings.SectionName)
            .Get<RedisSettings>()
            ?.ConnectionString;

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(connectionString));
            services.AddSingleton<ISessionSnapshotStore, RedisSessionSnapshotStore>();
            return services;
        }

        if (environment.IsDevelopment())
        {
            services.AddSingleton<ISessionSnapshotStore, InMemorySessionSnapshotStore>();
            return services;
        }

        throw new InvalidOperationException(
            "Redis:ConnectionString is required outside Development. " +
            "Configure Redis or run the API in Development to use the in-memory session store.");
    }
}
