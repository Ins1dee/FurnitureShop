using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Idempotency;
using Infrastructure.Mapping;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.OptionSetup;
using Infrastructure.Services.Email;
using Infrastructure.Services.Idempotency;
using Infrastructure.Services.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

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
        services.AddSingleton<ICashService, CasheService>();
        services.AddSingleton<IMapper, Mapper>();
        
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