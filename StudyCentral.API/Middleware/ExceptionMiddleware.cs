using System.Net;
using System.Security;
using System.Text.Json;

namespace StudyCentral.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
            var statusCode = GetStatusCode(ex);

            if ((int)statusCode >= 500)
            {
                _logger.LogError(ex, ex.Message);
            }
            else
            {
                _logger.LogWarning(ex.Message);
            }

            await HandleExceptionAsync(context, ex, statusCode);
        }
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ConflictException => HttpStatusCode.Conflict, // 409
            TimeoutException => HttpStatusCode.RequestTimeout, // 408
            KeyNotFoundException => HttpStatusCode.NotFound, // 404
            SecurityException => HttpStatusCode.Forbidden, // 403
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
            ArgumentException => HttpStatusCode.BadRequest, // 400
            InternalException => HttpStatusCode.InternalServerError, // 500
            NotImplementedException => HttpStatusCode.NotImplemented, // 501
            _ => HttpStatusCode.InternalServerError // 500
        };
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message = exception.Message
        };

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }

    public class InternalException : Exception
    {
        public InternalException(string message) : base(message)
        {
        }
    }
}