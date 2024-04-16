using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities.Categories;
using Domain.Entities.Deliveries;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Suppliers;
using Domain.Entities.SupplyProducts;
using Domain.Entities.UserRegistrations;
using Domain.Entities.Users;
using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

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
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<Delivery> Deliveries { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<SupplyProduct> SupplyProducts { get; set; }

    public DbSet<WarehouseProduct> WarehouseProducts { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

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