using Microsoft.AspNetCore.Mvc;
using TestAPI.ResultPattern;


namespace TestAPI.Extensions {

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return MapError(result.Error!);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return MapError(result.Error!);
    }

    private static IActionResult MapError(Error error) => error.ErrorType switch
    {
        ErrorType.NotFound     => new NotFoundObjectResult(error.Description),
        ErrorType.Conflict     => new ConflictObjectResult(error.Description),
        ErrorType.Validation   => new UnprocessableEntityObjectResult(error.Description),
        ErrorType.Unauthorized => new UnauthorizedObjectResult(error.Description),
        _                      => new ObjectResult(error.Description) { StatusCode = 500 }
    };
}

}
