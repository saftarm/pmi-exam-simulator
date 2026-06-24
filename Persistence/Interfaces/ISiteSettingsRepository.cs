using TestAPI.DTO.Settings;
using TestAPI.Entities;

namespace TestAPI.Persistence.Interfaces;

public interface ISiteSettingsRepository
{
    Task<(SiteSettings Settings, bool Created)> GetOrCreateAsync(CancellationToken ct = default);
    Task UpdateAsync(SiteSettings settings, CancellationToken ct = default);
}
