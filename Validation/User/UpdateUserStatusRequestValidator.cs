using FluentValidation;
using TestAPI.DTO.User;

namespace TestAPI.Validation.User
{
    public class UpdateUserStatusRequestValidator : AbstractValidator<UpdateUserStatusRequest>
    {
        public UpdateUserStatusRequestValidator()
        {
            RuleFor(r => r.Status).IsInEnum();
        }
    }
}
