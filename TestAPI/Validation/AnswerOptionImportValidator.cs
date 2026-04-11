using FluentValidation;
using TestAPI.DTO.ImportService;

namespace TestAPI.Validation
{
    public class AnswerOptionImportValidator : AbstractValidator<AnswerOptionImportDto>
    {

        public AnswerOptionImportValidator()
        {
            RuleFor(o => o.Text).NotEmpty().WithMessage("Answer Option Text is required");
            RuleFor(o => o.IsCorrect).NotEmpty().WithMessage("Answer Option flag is required");
        }
    }
}
