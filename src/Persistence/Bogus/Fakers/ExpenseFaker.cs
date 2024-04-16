using Bogus;
using Domain.Entities.Expenses;
using Domain.Entities.SupplyProducts;

namespace Persistence.Bogus.Fakers;

public sealed class ExpenseFaker : Faker<Expense>
{
    public ExpenseFaker(List<SupplyProduct> supplyProducts)
    {
        RuleFor(e => e.Id, f => new ExpenseId(f.Random.Guid()));
        RuleFor(e => e.CreatedAtUtc, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2023, 12, 30)));
        RuleFor(e => e.SupplyProductId, f => f.PickRandom(supplyProducts).Id);
    }
}