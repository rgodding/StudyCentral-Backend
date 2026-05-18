using System.Net;
using System.Security;
using System.Text.Json;

namespace StudyCentral.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ConflictException => (int)HttpStatusCode.Conflict, // 409
            TimeoutException => (int)HttpStatusCode.RequestTimeout, // 408
            KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
            SecurityException => (int)HttpStatusCode.Forbidden, // 403
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
            ArgumentException => (int)HttpStatusCode.BadRequest, // 400
            NotImplementedException => (int)HttpStatusCode.NotImplemented, // 501
            _ => (int)HttpStatusCode.InternalServerError // 500
        };

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = exception.Message
        };

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
    
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}