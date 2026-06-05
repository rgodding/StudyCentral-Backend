using System.Net;
using System.Security;
using System.Text.Json;

namespace StudyCentral.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var statusCode = MapExceptionToStatusCode(ex);

            LogException(ex, statusCode);

            await WriteResponseAsync(context, ex, statusCode);
        }
    }

    // -------------------------
    // Exception → HTTP mapping
    // -------------------------
    private static HttpStatusCode MapExceptionToStatusCode(Exception exception)
    {
        return exception switch
        {
            ConflictException => HttpStatusCode.Conflict, // 409
            KeyNotFoundException => HttpStatusCode.NotFound, // 404
            SecurityException => HttpStatusCode.Forbidden, // 403
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
            ArgumentException => HttpStatusCode.BadRequest, // 400
            TimeoutException => HttpStatusCode.RequestTimeout, // 408
            NotImplementedException => HttpStatusCode.NotImplemented, // 501
            InternalException => HttpStatusCode.InternalServerError, // 500
            _ => HttpStatusCode.InternalServerError
        };
    }

    // -------------------------
    // Logging
    // -------------------------
    private void LogException(Exception ex, HttpStatusCode statusCode)
    {
        if ((int)statusCode >= 500)
        {
            _logger.LogError(ex, "Unhandled server exception");
        }
        else
        {
            _logger.LogWarning("Handled exception: {Message}", ex.Message);
        }
    }

    // -------------------------
    // Response formatting
    // -------------------------
    private async Task WriteResponseAsync(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message = GetSafeMessage(exception, statusCode),
            errorType = exception.GetType().Name,
            traceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    // -------------------------
    // Safe message handling
    // -------------------------
    private string GetSafeMessage(Exception exception, HttpStatusCode statusCode)
    {
        // In production hide internal errors
        if (!_env.IsDevelopment() && (int)statusCode >= 500)
            return "An unexpected error occurred.";

        return exception.Message;
    }

    // -------------------------
    // Custom exceptions
    // -------------------------
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class InternalException : Exception
    {
        public InternalException(string message) : base(message) { }

        public InternalException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}