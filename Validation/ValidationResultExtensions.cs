using FluentValidation.Results;
using TestAPI.ResultPattern;

namespace TestAPI.Validation;

public static class ValidationResultExtensions
{
    public static Error ToError(this ValidationResult validationResult) =>
        Errors.FromValidation(validationResult);
}
