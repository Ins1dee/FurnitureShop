using Bogus;
using Domain.Entities.Suppliers;
using Domain.Entities.Suppliers.ValueObjects;

namespace Persistence.Bogus.Fakers;

public sealed class SupplierFaker : Faker<Supplier>
{
    public SupplierFaker()
    {
        RuleFor(s => s.Id, f => new SupplierId(f.Random.Guid()));
        RuleFor(s => s.CompanyName, f => CompanyName.Create(f.Company.CompanyName()));
        RuleFor(s => s.ContactDetails,
            f => ContactDetails
                .Create(f.Person.FullName, f.Phone.PhoneNumber(), f.Person.Email));
    }
}
