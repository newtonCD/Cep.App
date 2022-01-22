using Microsoft.Extensions.DependencyInjection;

namespace Cep.App.Infrastructure.Common.Extensions;

public static class HealthCheckExtensions
{
    internal static IServiceCollection AddHealthCheckExtension(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }
}