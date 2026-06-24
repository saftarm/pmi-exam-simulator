using TestAPI.DTO.Public;

namespace TestAPI.Persistence.Interfaces;

public interface IPublicStatsRepository
{
    Task<PublicStatsDto> GetStatsAsync(CancellationToken ct = default);
}
