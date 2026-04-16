
using FluentValidation;
using TestAPI.DTO;

namespace TestAPI.Validation
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(r => r.AccessToken).NotNull().NotEmpty().WithMessage("Access Token Text is required");
        }
    }
}
