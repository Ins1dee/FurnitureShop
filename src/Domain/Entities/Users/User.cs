using Domain.Abstractions;
using Domain.Entities.Orders;
using Domain.Entities.RefreshSessions;
using Domain.Entities.Roles;
using Domain.Entities.UserRegistrations;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Users;

public sealed class User : Entity<UserId>, IAggregateRoot
{
    private readonly List<Role> _roles = new();

    private readonly List<RefreshSession> _refreshSessions = new();

    private readonly List<Order> _orders = new();
    
    private User(
        UserId id,
        FullName fullName, 
        Email email, 
        PasswordHash passwordHash, 
        Role role) 
        : base(id)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        _roles.Add(role);
    }

    private User()
    {
        // For EF Core
    }
    
    public FullName FullName { get; private set; }
    
    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }
    
    public IReadOnlyList<Role> Roles => _roles;

    public IReadOnlyList<RefreshSession> RefreshSessions => _refreshSessions;

    public IReadOnlyList<Order> Orders => _orders;

    public static User CreateFromUserRegistration(
        UserRegistrationId id,
        FullName fullName,
        Email email,
        PasswordHash passwordHash)
    {
        return new User(new UserId(id.Value), fullName, email, passwordHash, Role.Customer);
    }

    public RefreshSession CreateRefreshSession()
    {
        RefreshSession refreshSession = RefreshSession.Create(
            new RefreshSessionId(Guid.NewGuid()),
            Id);
        
        _refreshSessions.Add(refreshSession);

        return refreshSession;
    }

    public Result<RefreshSession> UpdateRefreshSession(string? refreshToken)
    {
        if (refreshToken is null)
        {
            return Result.BadRequest<RefreshSession>(DomainErrors.User.RefreshSessionExpired());
        }
        
        RefreshSession? refreshSession = _refreshSessions
            .FirstOrDefault(x => x.Id.Value == Guid.Parse(refreshToken));

        if (refreshSession is null)
        {
            return Result.BadRequest<RefreshSession>(DomainErrors.User.InvalidRefreshToken());
        }

        _refreshSessions.Remove(refreshSession);

        RefreshSession updatedSession = CreateRefreshSession();

        return updatedSession;
    }
}