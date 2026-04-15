using FluentValidation;
using TestAPI.DTO.Auth.Requests;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Validation.Authentication
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserRequestValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Display name is required.")
                .MaximumLength(50).WithMessage("Display name cannot exceed 50 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_]*$").WithMessage("Username can only contain letters, numbers, and underscores.")
                .MustAsync(async (username, ct) => await _userRepository.IsUserNameUniqueAsync(username, ct))
                .WithMessage("This username is already taken.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.")
                .MustAsync(async (email, cancellation) => await _userRepository.IsEmailUniqueAsync(email, cancellation))
                .WithMessage("This email is already in use.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\?\*\.\#\$\%\&]").WithMessage("Password must contain a special character.");

        }


    }
}
