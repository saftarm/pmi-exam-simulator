using FluentValidation;
using TestAPI.DTO;
using TestAPI.DTO.ImportService;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation
{
    public class UpdateQuestionRequestValidator : AbstractValidator<UpdateQuestionRequest>
    {
      private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionRequestValidator(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;

            RuleFor(r => r.Id)
                  .MustAsync(async (id, ct) => 
                  await _questionRepository.ExistsAsync(id, ct));

            RuleFor(r => r.Title)
                  .NotEmpty().NotNull().WithMessage("Question Title required")
                  .MaximumLength(1000).WithMessage("Question Title cannot contain more than 1000 letters");
        }
    }
}
