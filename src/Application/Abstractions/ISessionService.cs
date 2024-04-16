using Domain.Entities.RefreshSessions;
using Domain.Entities.Users;

namespace Application.Abstractions;

public interface ISessionService
{
    Task<User?> GetLoggedInUserAsync(CancellationToken cancellationToken = default);

    void SetSessionInCookies(RefreshSession refreshSession);

    string? GetCurrentSessionFromCookies();

    void DeleteCurrentSessionFromCookies();
}