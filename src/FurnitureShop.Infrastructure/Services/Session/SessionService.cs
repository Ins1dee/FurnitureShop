using System.Security.Claims;
using FurnitureShop.Application.Abstractions;
using FurnitureShop.Domain.Entities.RefreshSessions;
using FurnitureShop.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace FurnitureShop.Infrastructure.Services.Session;

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

    public async Task<User> GetLoggedInUserAsync()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        return (email is not null
            ? await _userRepository.GetByEmailAsync(email)
            : null)!;
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