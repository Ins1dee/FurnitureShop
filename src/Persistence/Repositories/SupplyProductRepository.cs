using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Suppliers;
using Domain.Entities.SupplyProducts;

namespace Persistence.Repositories;
internal class SupplyProductRepository : Repository<SupplyProduct, SupplyProductId>, ISupplyProductRepository
{
    public SupplyProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
