using Domain.Entities.Users;

namespace Domain.Entities.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);
    
    void Delete(Order order);
    Task<List<Order>> GetAsync(UserId userId, CancellationToken cancellationToken = default);
}