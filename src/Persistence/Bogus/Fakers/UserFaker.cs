using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Domain.Entities.Products.ValueObjects;
using Domain.Entities.Products;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Shared.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class UserFaker: Faker<User>
{
    private readonly List<Role> _roles = new() {Role.Administrator, Role.Delivery, Role.Seller};

    public UserFaker()
    {
        CustomInstantiator(f =>
        {
            var randomRoles = new List<Role>{ Role.Seller };

            var user = User.Create(
                FullName.Create("Test", "Test"),
                Email.Create(f.Person.Email),
                PasswordHash.Create(f.Internet.Password()),
                randomRoles);

            return user;
        });

        RuleFor(u => u.Id, f => new UserId(f.Random.Guid()));
        RuleFor(u => u.Email, f => Email.Create(f.Person.Email));
        RuleFor(u => u.FullName, f => FullName.Create(f.Person.FirstName, f.Person.LastName)); 
        RuleFor(u => u.PasswordHash, PasswordHash.Hash("Password@1"));
    }
}