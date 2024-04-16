using Domain.Abstractions;
using Domain.Entities.Deliveries;
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

    private readonly List<Delivery> _deliveries = new();
    
    private User(
        UserId id,
        FullName fullName, 
        Email email, 
        PasswordHash passwordHash, 
        List<Role> roles) 
        : base(id)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        _roles.AddRange(roles);
    }

    public User()
    {
        // For EF Core
    }
    
    public FullName FullName { get; private set; }
    
    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }
    
    public IReadOnlyList<Role> Roles => _roles;

    public IReadOnlyList<RefreshSession> RefreshSessions => _refreshSessions;

    public IReadOnlyList<Order> Orders => _orders;

    public IReadOnlyList<Delivery> Deliveries => _deliveries;

    public static User CreateFromUserRegistration(
        UserRegistrationId id,
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        Role role)
    {
        return new User(new UserId(id.Value), fullName, email, passwordHash, new List<Role>{ role });
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

    public static User Create(
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        List<Role> roles)
    {
        return new User(new UserId(Guid.NewGuid()), fullName, email, passwordHash, roles);
    }
}