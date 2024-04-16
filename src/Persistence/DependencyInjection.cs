using Application.Abstractions;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Bogus;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => 
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), 
                    x => x.MigrationsAssembly("Persistence")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplyProductRepository, SupplyProductRepository>();
        services.AddScoped<IWarehouseProductRepository, WarehouseProductRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();

        services.AddScoped<IDataSeeder, DataSeeder>();
        
        return services;
    }
}