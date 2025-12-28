using Microsoft.AspNetCore.Mvc;
using System.Net;
using TamweelyHR.Application.Exceptions;

namespace TamweelyHr.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = exception switch
            {
                NotFoundException notFound => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not Found",
                    Detail = notFound.Message,
                    Instance = context.Request.Path
                },
                DuplicateException duplicate => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Conflict,
                    Title = "Duplicate Entry",
                    Detail = duplicate.Message,
                    Instance = context.Request.Path
                },
                UnauthorizedAccessException => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Title = "Unauthorized",
                    Detail = "Invalid credentials",
                    Instance = context.Request.Path
                },
                FluentValidation.ValidationException validation => new ValidationProblemDetails(
                    validation.Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Validation Error",
                    Instance = context.Request.Path
                },
                _ => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing your request",
                    Instance = context.Request.Path
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problemDetails.Status ?? 500;

            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
