namespace Domain.Entities.Deliveries;

public interface IDeliveryRepository
{
    Task AddRangeAsync(List<Delivery> deliveries,  CancellationToken cancellationToken = default);
    Task AddAsync(Delivery delivery, CancellationToken cancellationToken = default);
}