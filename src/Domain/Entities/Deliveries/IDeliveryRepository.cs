using Domain.Entities.Users;

namespace Domain.Entities.Deliveries;

public interface IDeliveryRepository
{
    Task AddRangeAsync(List<Delivery> deliveries,  CancellationToken cancellationToken = default);

    Task AddAsync(Delivery delivery, CancellationToken cancellationToken = default);

    Task<Delivery?> GetByIdAsync(DeliveryId deliveryId, CancellationToken cancellationToken = default);
    
    Task<List<Delivery>> GetAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<List<Delivery>> GetAllAsync(CancellationToken cancellationToken = default);
}