using Bogus;
using Domain.Entities.Incomes;
using Domain.Entities.Orders;

namespace Persistence.Bogus.Fakers;

public sealed class IncomeFaker: Faker<Income>
{
    public IncomeFaker(List<Order> orders)
    {
        RuleFor(inc => inc.Id, f => new IncomeId(f.Random.Guid()));
        RuleFor(inc => inc.CreatedAtUtc, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2023, 12, 30)));
    }
}