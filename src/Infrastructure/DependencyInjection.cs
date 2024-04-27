using System.Collections.ObjectModel;
using System.Data;
using Application.Abstractions;
using Application.Abstractions.Caching;
using Application.Abstractions.Idempotency;
using Infrastructure.Mapping;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.OptionSetup;
using Infrastructure.Reports;
using Infrastructure.Services.Email;
using Infrastructure.Services.Idempotency;
using Infrastructure.Services.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)

    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<IReportService, ReportService>();
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

        services.AddSerilog(options =>
        {
            options.MinimumLevel.Information();
            options.MinimumLevel.Override("Microsoft", LogEventLevel.Error);
            options
                .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString("sqlConnection"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "LogEvents",
                    AutoCreateSqlTable = true
                },
                columnOptions: new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                    {
                        new()
                        {
                            ColumnName = "LoggedUser",
                            PropertyName = "LoggedUser",
                            DataType = SqlDbType.NVarChar
                        }
                    }
                });
        });

        return services;
    }
}