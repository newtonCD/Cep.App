using Cep.App.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Cep.App.Infrastructure.Common.Extensions;

public static class SignalrExtensions
{
    internal static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        ILogger logger = Log.ForContext(typeof(SignalrExtensions));

        var signalSettings = services.GetOptions<SignalRSettings>("SignalRSettings");

        if (!signalSettings.UseBackplane)
        {
            services.AddSignalR();
        }
        else
        {
            var backplaneSettings = services.GetOptions<SignalRSettings.Backplane>("SignalRSettings:Backplane");
            switch (backplaneSettings.Provider)
            {
                case "redis":
                    services.AddSignalR().AddStackExchangeRedis(backplaneSettings.StringConnection, options =>
                    {
                        options.Configuration.AbortOnConnectFail = false;
                    });
                    break;

                default:
                    throw new InvalidOperationException($"SignalR backplane Provider {backplaneSettings.Provider} is not supported.");
            }

            logger.Information($"SignalR Backplane Current Provider: {backplaneSettings.Provider}.");
        }

        return services;
    }
}