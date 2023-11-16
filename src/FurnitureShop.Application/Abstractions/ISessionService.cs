using FurnitureShop.Domain.Entities.RefreshSessions;
using FurnitureShop.Domain.Entities.Users;

namespace FurnitureShop.Application.Abstractions;

public interface ISessionService
{
    Task<User> GetLoggedInUserAsync();

    void SetSessionInCookies(RefreshSession refreshSession);

    string? GetCurrentSessionFromCookies();

    void DeleteCurrentSessionFromCookies();
}