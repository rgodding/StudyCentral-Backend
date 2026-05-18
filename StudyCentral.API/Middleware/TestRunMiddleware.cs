using StudyCentral.API.Models;

namespace StudyCentral.API.Middleware;

public class TestRunMiddleware
{
    private readonly RequestDelegate _next;

    public TestRunMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, StudyDbContext db)
    {
        if (context.Request.Headers["X-Test-Run"] == "true")
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            await _next(context);

            await transaction.RollbackAsync();
        }
        else
        {
            await _next(context);
        }
    }
}