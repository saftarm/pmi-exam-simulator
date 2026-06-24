using FluentValidation;
using TestAPI.DTO;

namespace TestAPI.Validation.Exam;

public class CreateExamDtoValidator : AbstractValidator<CreateExamDto>
{
    public CreateExamDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.DurationInMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(360);

        RuleFor(x => x.NumberOfQuestions)
            .GreaterThan(0);

        RuleFor(x => x.CreateDomainDtos)
            .NotEmpty()
            .WithMessage("At least one domain is required.");

        RuleFor(x => x.CreateDomainDtos)
            .Must(domains => domains.Sum(d => d.Weight) is >= 99 and <= 101)
            .WithMessage("Domain weights must sum to approximately 100.");
    }
}
