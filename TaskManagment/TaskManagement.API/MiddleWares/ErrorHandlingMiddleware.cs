using System.Net;
using System.Text.Json;
using TaskManagemnt.Domain.Exceptions;
using TaskManagment.Application.Exceptions;

namespace TaskManagement.API.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log exception (this was missing)
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            object responseData;

            // Default response
            responseData = new
            {
                error = exception.Message,
                detail = exception.StackTrace
            };

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    responseData = new { error = exception.Message };
                    break;

                case BusinessRuleException businessRuleException:
                    statusCode = HttpStatusCode.BadRequest;
                    responseData = JsonSerializer.Serialize(businessRuleException.Message);
                    break;
                case CustomValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    responseData = JsonSerializer.Serialize(validationException.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(responseData));
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
