using Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Suppliers;

namespace Persistence.Repositories;
internal class SupplierRepository : Repository<Supplier, SupplierId>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
