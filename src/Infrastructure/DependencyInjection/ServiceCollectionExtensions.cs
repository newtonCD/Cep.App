using AspNetCoreRateLimit;
using Cep.App.Application.Catalog.Interfaces;
using Cep.App.Application.Common.Interfaces;
using Cep.App.Infrastructure.Common.Extensions;
using Cep.App.Infrastructure.Common.Services;
using Cep.App.Infrastructure.HangFire;
using Cep.App.Infrastructure.Localizer;
using Cep.App.Infrastructure.Persistence;
using Cep.App.Infrastructure.Persistence.Repositories;
using Cep.App.Infrastructure.Swagger;
using Hangfire;
using Hangfire.Console.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Cep.App.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        if (config.GetSection("CacheSettings:PreferRedis").Get<bool>())
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetSection("CacheSettings:RedisURL").Get<string>();
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    AbortOnConnectFail = true
                };
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.TryAdd(ServiceDescriptor.Singleton<ICacheService, CacheService>());

        // Rate Limit
        if (config.GetSection("IpRateLimitSettings:EnableEndpointRateLimiting").Get<bool>())
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(config.GetSection("IpRateLimitSettings"));
            services.Configure<IpRateLimitPolicies>(config.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Caso esteja utilizando load-balance, deve-se alterar as implementações
            // abaixo para usar DistributedCacheIpPolicyStore e DistributedCacheRateLimitCounterStore
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }

        services.TryAdd(ServiceDescriptor.Singleton<ISqlDataAccess, SqlDataAccess>());
        services.TryAdd(ServiceDescriptor.Singleton<IAddressRepository, AddressRepository>());

        services.AddHealthCheckExtension();
        services.AddLocalization();
        services.AddServices();
        services.AddSettings(config);

        services.AddHangfireServer(options =>
        {
            var optionsServer = services.GetOptions<BackgroundJobServerOptions>("HangFireSettings:Server");
            options.HeartbeatInterval = optionsServer.HeartbeatInterval;
            options.Queues = optionsServer.Queues;
            options.SchedulePollingInterval = optionsServer.SchedulePollingInterval;
            options.ServerCheckInterval = optionsServer.ServerCheckInterval;
            options.ServerName = optionsServer.ServerName;
            options.ServerTimeout = optionsServer.ServerTimeout;
            options.ShutdownTimeout = optionsServer.ShutdownTimeout;
            options.WorkerCount = optionsServer.WorkerCount;
        });
        services.AddHangfireConsoleExtensions();
        services.AddHangFireService();

        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddMiddlewares();
        services.AddSwaggerDocumentation();
        services.AddCorsPolicy();

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        services.TryAdd(ServiceDescriptor.Singleton<IStringLocalizerFactory, JsonStringLocalizerFactory>());

        return services;
    }
}