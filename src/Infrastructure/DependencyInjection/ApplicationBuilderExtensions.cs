using AspNetCoreRateLimit;
using Cep.App.Infrastructure.Common.Extensions;
using Cep.App.Infrastructure.Swagger;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Cep.App.Api")]

namespace Cep.App.Infrastructure.DependencyInjection;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("IpRateLimitSettings:EnableEndpointRateLimiting")) app.UseIpRateLimiting();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
        });

        app.UseMiddlewares(config);
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseHttpsRedirection();
        app.UseAuthorization();

        var configDashboard = config.GetSection("HangFireSettings:Dashboard").Get<DashboardOptions>();
        app.UseHangfireDashboard(config["HangFireSettings:Route"], new DashboardOptions
        {
            DashboardTitle = configDashboard.DashboardTitle,
            StatsPollingInterval = configDashboard.StatsPollingInterval,
            AppPath = configDashboard.AppPath,
            Authorization = new[]
            {
                new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = config["HangFireSettings:Credentials:User"],
                            PasswordClear = config["HangFireSettings:Credentials:Password"]
                        }
                    }
                })
            }
        });

        app.UseSwaggerDocumentation(config);

        return app;
    }
}