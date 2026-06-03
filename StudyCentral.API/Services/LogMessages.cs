using System.Net;

namespace StudyCentral.API.Services;

public static class LogMessages
{
    public static string Format(
        LogSeverity severity,
        HttpStatusCode statusCode,
        string message)
    {
        return $"[{severity}]_[{(int)statusCode}]: {message}";
    }
    
}

public enum LogSeverity
{
    Information,
    Warning,
    Error
}