using FurnitureShop.Application.Abstractions;
using FurnitureShop.Domain.Entities.Categories;
using FurnitureShop.Domain.Entities.Products;
using FurnitureShop.Domain.Entities.UserRegistrations;
using FurnitureShop.Domain.Entities.Users;
using FurnitureShop.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureShop.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => 
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), 
                    x => x.MigrationsAssembly("FurnitureShop.Persistence")));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        return services;
    }
}