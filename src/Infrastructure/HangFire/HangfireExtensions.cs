using Cep.App.Application.Settings;
using Cep.App.Infrastructure.Common.Extensions;
using Hangfire;
using Hangfire.Console;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Cep.App.Infrastructure.HangFire;

public static class HangfireExtensions
{
    private static readonly ILogger _logger = Log.ForContext(typeof(HangfireExtensions));

    public static IServiceCollection AddHangFireService(this IServiceCollection services)
    {
        var storageSettings = services.GetOptions<HangFireStorageSettings>("HangFireSettings:Storage");

        if (string.IsNullOrEmpty(storageSettings.StorageProvider))
            throw new InvalidOperationException("Storage HangFire Provider is not configured.");

        _logger.Information($"Hangfire: Current Storage Provider : {storageSettings.StorageProvider}");

        services.AddSingleton<JobActivator, ContextJobActivator>();

        switch (storageSettings.StorageProvider.ToLower())
        {
            case "postgresql":
                services.AddHangfire((_, config) =>
                {
                    config.UsePostgreSqlStorage(storageSettings.ConnectionString, services.GetOptions<PostgreSqlStorageOptions>("HangFireSettings:Storage:Options"))
                    .UseFilter(new LogJobFilter())
                    .UseConsole();
                });
                break;

            case "mssql":
                services.AddHangfire((_, config) =>
                {
                    config.UseSqlServerStorage(storageSettings.ConnectionString, services.GetOptions<SqlServerStorageOptions>("HangFireSettings:Storage:Options"))
                    .UseFilter(new LogJobFilter())
                    .UseConsole();
                });
                break;

            case "mysql":
                services.AddHangfire((_, config) =>
                {
                    config.UseStorage(new MySqlStorage(storageSettings.ConnectionString, services.GetOptions<MySqlStorageOptions>("HangFireSettings:Storage:Options")))
                    .UseFilter(new LogJobFilter())
                    .UseConsole();
                });
                break;

            default:
                throw new InvalidOperationException($"HangFire Storage Provider {storageSettings.StorageProvider} is not supported.");
        }

        return services;
    }
}