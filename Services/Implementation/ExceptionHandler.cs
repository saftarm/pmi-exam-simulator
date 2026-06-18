using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Exceptions;

namespace TestAPI.Services
{



    public class ExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            _logger.LogError(exception, "Exception occured: {Message}", exception.Message);


            var statusCode = exception switch
            {
                RecordNotFoundException => StatusCodes.Status404NotFound,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                FormatException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,

                ValidationException => StatusCodes.Status422UnprocessableEntity,

                _ => StatusCodes.Status500InternalServerError



            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Detail = exception.Message

            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }









    }












}