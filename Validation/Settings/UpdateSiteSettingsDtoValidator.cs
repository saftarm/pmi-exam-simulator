using FluentValidation;
using TestAPI.DTO.Settings;

namespace TestAPI.Validation.Settings;

public class UpdateSiteSettingsDtoValidator : AbstractValidator<UpdateSiteSettingsDto>
{
    public UpdateSiteSettingsDtoValidator()
    {
        RuleFor(x => x.SiteName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SupportEmail).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.DefaultExamDuration).InclusiveBetween(1, 360);
        RuleFor(x => x.PassThreshold).InclusiveBetween(0, 100);
    }
}
