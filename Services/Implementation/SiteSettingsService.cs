using TestAPI.DTO.Settings;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation;

public class SiteSettingsService(
    ISiteSettingsRepository repository,
    IValidatorResolver validatorResolver) : ISiteSettingsService
{
    private readonly ISiteSettingsRepository _repository = repository;
    private readonly IValidatorResolver _validatorResolver = validatorResolver;

    public async Task<Result<SiteSettingsDto>> GetAsync(CancellationToken ct = default)
    {
        var settings = await _repository.GetOrCreateAsync(ct);
        return Result<SiteSettingsDto>.Success(MapToDto(settings));
    }

    public async Task<Result<SiteSettingsDto>> UpdateAsync(UpdateSiteSettingsDto dto, CancellationToken ct = default)
    {
        var validationResult = await _validatorResolver.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Result<SiteSettingsDto>.Failure(Errors.ValidationFailed);
        }

        var settings = await _repository.GetOrCreateAsync(ct);
        settings.SiteName = dto.SiteName;
        settings.SupportEmail = dto.SupportEmail;
        settings.AllowRegistration = dto.AllowRegistration;
        settings.MaintenanceMode = dto.MaintenanceMode;
        await _repository.UpdateAsync(settings, ct);
        return Result<SiteSettingsDto>.Success(MapToDto(settings));
    }

    private static SiteSettingsDto MapToDto(Entities.SiteSettings settings) => new()
    {
        SiteName = settings.SiteName,
        SupportEmail = settings.SupportEmail,
        AllowRegistration = settings.AllowRegistration,
        MaintenanceMode = settings.MaintenanceMode,
    };
}
