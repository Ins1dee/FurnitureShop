using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses;

namespace Persistence.Repositories;
internal class WarehouseRepository : Repository<Warehouse, WarehouseId>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
