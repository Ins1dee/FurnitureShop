using System.Reflection;
using FluentValidation;
using FurnitureShop.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));

            cfg.AddOpenBehavior(typeof(IdempotentCommandPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}