using Domain.Entities.Categories;
using Domain.Entities.Deliveries;

namespace Persistence.Repositories;

internal class DeliveryRepository : Repository<Delivery, DeliveryId>, IDeliveryRepository
{
    public DeliveryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}