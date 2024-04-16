using Domain.Entities.Orders;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class OrderRepository : Repository<Order, OrderId>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public void Delete(Order order)
    {
        DbContext.Orders.Remove(order);
    }

    public async Task<List<Order>> GetAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(o => o.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Orders.Include(o => o.Incomes).ToListAsync(cancellationToken: cancellationToken);
    }
}