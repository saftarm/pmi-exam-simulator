using TestAPI.DTO.Public;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces;

public interface IPublicStatsService
{
    Task<Result<PublicStatsDto>> GetStatsAsync(CancellationToken ct = default);
}
