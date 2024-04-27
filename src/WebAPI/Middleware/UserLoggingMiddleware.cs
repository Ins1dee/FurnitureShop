using System.Security.Claims;
using Application.Abstractions;
using Serilog.Context;

namespace WebAPI.Middleware;

public class UserLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public UserLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ISessionService _sessionService)
    {
        var userEmail = context.User.FindFirstValue(ClaimTypes.Email);
        
        using (LogContext.PushProperty("LoggedUser", userEmail))
        {
            await _next.Invoke(context);
        }
    }
}
