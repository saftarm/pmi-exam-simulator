using FluentValidation;
using TestAPI.DTO.Question;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation.Question
{
    public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionDtoValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(r => r.Explanation)
                .MaximumLength(1000);

            RuleFor(r => r.DomainId)
                .NotEmpty();

            RuleFor(r => r.AnswerOptionsDtos)
                .NotEmpty()
                .WithMessage("At least one answer option is required");
        }
    }
}
