using System.Security.Claims;
using Application.Abstractions;
using Domain.Entities.RefreshSessions;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Session;

public class SessionService : ISessionService
{
    private const string RefreshTokenKey = "refreshToken";
    
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User?> GetLoggedInUserAsync(CancellationToken cancellationToken = default)
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        if (email is null)
        {
            return null;
        }
        
        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        return user;
    }

    public void SetSessionInCookies(RefreshSession refreshSession)
    {
        CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            Expires = refreshSession.ExpiresAtUtc
        };

        _httpContextAccessor.HttpContext?.Response.Cookies
            .Append(RefreshTokenKey, refreshSession.Id.Value.ToString(), cookieOptions);
    }
    
    public string? GetCurrentSessionFromCookies()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenKey];
    }

    public void DeleteCurrentSessionFromCookies()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies
            .Delete(RefreshTokenKey);
    }
}