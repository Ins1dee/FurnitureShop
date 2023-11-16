using FurnitureShop.Application.Abstractions;
using FurnitureShop.Domain.Abstractions;
using FurnitureShop.Domain.Entities.Categories;
using FurnitureShop.Domain.Entities.Products;
using FurnitureShop.Domain.Entities.UserRegistrations;
using FurnitureShop.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FurnitureShop.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }
    
    public DbSet<UserRegistration> UserRegistrations { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        List<DomainEvent> domainEvents = ChangeTracker.Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.GetDomainEvents().Any())
            .SelectMany(entity =>
            {
                IEnumerable<DomainEvent> domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (DomainEvent domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}