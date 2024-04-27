using System.Reflection;
using Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));

            cfg.AddOpenBehavior(typeof(IdempotentCommandPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        License.LicenseKey = configuration["IronPdf:LicenseKey"];

        return services;
    }
}