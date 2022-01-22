using Cep.App.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cep.App.Infrastructure.Common.Extensions;

public static class SettingExtensions
{
    internal static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        services
            .Configure<MailSettings>(config.GetSection(nameof(MailSettings)))
            .Configure<MiddlewareSettings>(config.GetSection(nameof(MiddlewareSettings)))
            .Configure<CorsSettings>(config.GetSection(nameof(CorsSettings)))
            .Configure<CacheSettings>(config.GetSection(nameof(CacheSettings)))
            .Configure<SwaggerSettings>(config.GetSection(nameof(SwaggerSettings)))
            .Configure<IpRateLimitSettings>(config.GetSection(nameof(IpRateLimitSettings)));

        return services;
    }
}