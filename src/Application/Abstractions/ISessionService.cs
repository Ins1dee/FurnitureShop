using Domain.Entities.RefreshSessions;
using Domain.Entities.Users;

namespace Application.Abstractions;

public interface ISessionService
{
    Task<User> GetLoggedInUserAsync();

    void SetSessionInCookies(RefreshSession refreshSession);

    string? GetCurrentSessionFromCookies();

    void DeleteCurrentSessionFromCookies();
}