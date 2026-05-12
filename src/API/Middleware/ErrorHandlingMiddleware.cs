using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.API.Middleware
{
    internal sealed class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");

                var statusCode = ex switch
                {
                    DomainException => StatusCodes.Status400BadRequest,
                    SecurityException => StatusCodes.Status403Forbidden,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = "An error occured",
                    Status = statusCode,
                    Detail = statusCode == StatusCodes.Status500InternalServerError ? "An unexpected error occurred" : ex.Message,
                });
            }
        }
    }
}
