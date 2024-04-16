namespace Domain.Entities.Suppliers;

public interface ISupplierRepository
{
    Task AddRangeAsync(List<Supplier> suppliers, CancellationToken cancellationTolen = default);
}