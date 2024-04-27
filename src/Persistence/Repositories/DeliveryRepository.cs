using Domain.Entities.Categories;
using Domain.Entities.Deliveries;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class DeliveryRepository : Repository<Delivery, DeliveryId>, IDeliveryRepository
{
    public DeliveryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Delivery>> GetAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deliveries
            .Where(d => d.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Delivery>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Deliveries
            .Include(d => d.User)
            .Include(d => d.Order)
            .ToListAsync(cancellationToken);
    }
}