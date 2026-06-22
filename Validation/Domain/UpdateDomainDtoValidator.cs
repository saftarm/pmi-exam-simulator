using FluentValidation;
using TestAPI.DTO;

namespace TestAPI.Validation.Domain;

public class UpdateDomainDtoValidator : AbstractValidator<UpdateDomainDto>
{
    public UpdateDomainDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Weight).InclusiveBetween(0, 99);
    }
}
