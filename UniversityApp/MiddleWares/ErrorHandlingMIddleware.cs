using System.Net;
using System.Text.Json;

namespace UniversityApp.MiddleWares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");

            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred" });

            if (exception is HttpRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = "Bad request" });
            }
            else if (exception is NotImplementedException)
            {
                statusCode = (int)HttpStatusCode.NotImplemented;
                result = JsonSerializer.Serialize(new { error = "Not implemented" });
            }

            response.StatusCode = statusCode;

            return response.WriteAsync(result);
        }
    }
}