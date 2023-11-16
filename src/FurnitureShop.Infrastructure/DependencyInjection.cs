using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Abstractions.Idempotency;
using FurnitureShop.Infrastructure.Authentication;
using FurnitureShop.Infrastructure.Caching;
using FurnitureShop.Infrastructure.OptionSetup;
using FurnitureShop.Infrastructure.Services.Email;
using FurnitureShop.Infrastructure.Services.Idempotency;
using FurnitureShop.Infrastructure.Services.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddSingleton<CasheService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        return services;
    }
}