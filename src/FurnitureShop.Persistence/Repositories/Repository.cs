using FurnitureShop.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Persistence.Repositories;

internal abstract class Repository<TEntity, TEntityId> 
    where TEntity: Entity<TEntityId>, IAggregateRoot
    where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }
}