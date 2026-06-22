using TestAPI.DTO.Settings;
using TestAPI.ResultPattern;

namespace TestAPI.Services.Interfaces;

public interface ISiteSettingsService
{
    Task<Result<SiteSettingsDto>> GetAsync(CancellationToken ct = default);
    Task<Result<SiteSettingsDto>> UpdateAsync(UpdateSiteSettingsDto dto, CancellationToken ct = default);
}
