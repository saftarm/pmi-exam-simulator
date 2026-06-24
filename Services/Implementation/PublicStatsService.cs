using TestAPI.DTO.Public;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;

namespace TestAPI.Services.Implementation;

public class PublicStatsService(IPublicStatsRepository publicStatsRepository) : IPublicStatsService
{
    public async Task<Result<PublicStatsDto>> GetStatsAsync(CancellationToken ct = default)
    {
        var stats = await publicStatsRepository.GetStatsAsync(ct);
        return Result<PublicStatsDto>.Success(stats);
    }
}
