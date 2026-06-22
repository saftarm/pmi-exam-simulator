using FluentValidation;
using TestAPI.DTO.Settings;

namespace TestAPI.Validation.Settings;

public class UpdateSiteSettingsDtoValidator : AbstractValidator<UpdateSiteSettingsDto>
{
    public UpdateSiteSettingsDtoValidator()
    {
        RuleFor(x => x.SiteName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SupportEmail).NotEmpty().EmailAddress().MaximumLength(100);
    }
}
