using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Entities.SupplyProducts;
using Domain.Shared.ValueObjects;

namespace Domain.Entities.Expenses;

public sealed class Expense : Entity<ExpenseId>
{
    public Expense(
        SupplyProductId supplyProductId,
        ExpenseId id, 
        Amount amount, 
        DateTime createdAtUtc,
        SupplyProduct? supplyProduct) 
        : base(id)
    {
        Amount = amount;
        CreatedAtUtc = createdAtUtc;
        SupplyProduct = supplyProduct;
        SupplyProductId = supplyProductId;
    }

    public Expense()
    {
        // For EF
    }

    public SupplyProductId SupplyProductId { get; private set; }

    public Amount Amount { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public SupplyProduct? SupplyProduct { get; private set; }

    public void SetAmount(double amount)
    {
        Amount = Amount.Create(amount);
    }
}
