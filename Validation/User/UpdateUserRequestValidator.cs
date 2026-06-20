using FluentValidation;
using TestAPI.DTO.User;

namespace TestAPI.Validation.User
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(r => r.DisplayName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(r => r.Role).IsInEnum();
            RuleFor(r => r.Status).IsInEnum();
        }
    }
}
