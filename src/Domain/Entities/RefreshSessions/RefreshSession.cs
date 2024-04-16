using Domain.Abstractions;
using Domain.Entities.Users;

namespace Domain.Entities.RefreshSessions;

public class RefreshSession : Entity<RefreshSessionId>
{
    private const int ExpirationTimeInHours = 1;
    
    private RefreshSession(
        RefreshSessionId id,
        UserId userId,
        DateTime createdAtUtc,
        DateTime expiresAtUtc) 
        : base (id)
    {
        UserId = userId;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }
    
    public UserId UserId { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime ExpiresAtUtc { get; private set; }
    
    public static RefreshSession Create(RefreshSessionId id, UserId userId)
    {
        return new RefreshSession(id, userId, DateTime.UtcNow, DateTime.UtcNow.AddHours(ExpirationTimeInHours));
    }
}