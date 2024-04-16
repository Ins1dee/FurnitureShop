using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.WarehouseProducts;

namespace Persistence.Repositories;
internal class WarehouseProductRepository : Repository<WarehouseProduct, WarehouseProductId>, IWarehouseProductRepository
{
    public WarehouseProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
