using FluentValidation;
using TestAPI.DTO.User;
using TestAPI.Enums;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation.User
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator(IUserRepository userRepository)
        {
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.UserName).NotEmpty().MaximumLength(50);
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
            RuleFor(r => r.Role).IsInEnum();
            RuleFor(r => r.Status).IsInEnum();
        }
    }
}
