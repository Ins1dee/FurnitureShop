using Domain.Abstractions;
using Domain.Entities.Users;

namespace Domain.Entities.Roles;

public sealed class Role : Entity<RoleId>
{
    public static readonly Role Customer = new(new RoleId(nameof(Customer)));
    
    public static readonly Role Administrator = new(new RoleId(nameof(Administrator)));

    private Role(RoleId id) : base(id)
    {
    }

    public UserId? UserId { get; private set; }
}