using System.Diagnostics;
using Domain.Abstractions;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;

namespace Domain.Entities.Roles;

public sealed class Role
{
    public static Role Seller => new Role(nameof(Seller));

    public static Role Administrator => new(nameof(Administrator));

    public static Role Delivery = new(nameof(Delivery));

    public string Value { get; }

    private Role(string value)
    {
        this.Value = value;
    }

    public static Result<Role> FromString(string requestedRole)
    {
        return requestedRole switch
        {
            nameof(Seller) => Seller,
            nameof(Delivery) => Delivery,
            nameof(Administrator) => Administrator,
            _ => Result.NotFound<Role>(DomainErrors.Role.NotFound())
        };
    }
}