namespace TestAPI.Models.Session;

public class RedisSettings
{
    public const string SectionName = "Redis";

    public string? ConnectionString { get; set; }

    public string KeyPrefix { get; set; } = "session:snapshot:";
}
