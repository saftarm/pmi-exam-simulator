using FluentValidation;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation.Authentication
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty().NotNull().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("UserName cannot exceed 50 characters.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must contain at least 8 charachters");
        }

    }
}
