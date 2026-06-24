using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using TestAPI.ResultPattern;

namespace TestAPI.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        return MapError(result.Error!);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return MapError(result.Error!);
    }

    public static IActionResult ToValidationActionResult(this ValidationResult validationResult) =>
        MapError(Errors.FromValidation(validationResult));

    private static IActionResult MapError(Error error) => error.ErrorType switch
    {
        ErrorType.NotFound => new NotFoundObjectResult(error.Description),
        ErrorType.Conflict => new ConflictObjectResult(error.Description),
        ErrorType.Validation => new UnprocessableEntityObjectResult(
            error.ValidationErrors is { Count: > 0 }
                ? error.ValidationErrors.Select(e => new { propertyName = e.PropertyName, errorMessage = e.ErrorMessage })
                : new[] { new { propertyName = string.Empty, errorMessage = error.Description } }),
        ErrorType.UnprocessableEntity => new UnprocessableEntityObjectResult(error.Description),
        ErrorType.Unauthorized => new UnauthorizedObjectResult(error.Description),
        ErrorType.Forbidden => new ObjectResult(error.Description) { StatusCode = StatusCodes.Status403Forbidden },
        ErrorType.ServiceUnavailable => new ObjectResult(error.Description) { StatusCode = StatusCodes.Status503ServiceUnavailable },
        _ => new ObjectResult(error.Description) { StatusCode = 500 },
    };
}
