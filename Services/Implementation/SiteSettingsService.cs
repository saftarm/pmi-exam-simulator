using TestAPI.DTO.Settings;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;

namespace TestAPI.Services.Implementation;

public class SiteSettingsService(
    ISiteSettingsRepository repository,
    IValidatorResolver validatorResolver,
    IUnitOfWork unitOfWork) : ISiteSettingsService
{
    private readonly ISiteSettingsRepository _repository = repository;
    private readonly IValidatorResolver _validatorResolver = validatorResolver;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SiteSettingsDto>> GetAsync(CancellationToken ct = default)
    {
        var (settings, created) = await _repository.GetOrCreateAsync(ct);
        if (created)
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }

        return Result<SiteSettingsDto>.Success(MapToDto(settings));
    }

    public async Task<Result<SiteSettingsDto>> UpdateAsync(UpdateSiteSettingsDto dto, CancellationToken ct = default)
    {
        var validationResult = await _validatorResolver.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Result<SiteSettingsDto>.Failure(validationResult.ToError());
        }

        var (settings, created) = await _repository.GetOrCreateAsync(ct);
        settings.Update(dto.SiteName, dto.SupportEmail, dto.AllowRegistration, dto.MaintenanceMode);
        if (!created)
        {
            await _repository.UpdateAsync(settings, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);
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
