﻿using Cep.App.Application.Settings;
using Cep.App.Infrastructure.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cep.App.Infrastructure.Common.Extensions;

public static class MiddlewareExtensions
{
    internal static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        var middlewareSettings = services.GetOptions<MiddlewareSettings>(nameof(MiddlewareSettings));

        if (middlewareSettings.EnableLocalization) services.AddSingleton<LocalizationMiddleware>();
        if (middlewareSettings.EnableHttpsLogging) services.AddSingleton<RequestLoggingMiddleware>();
        services.AddScoped<ExceptionMiddleware>();
        if (middlewareSettings.EnableHttpsLogging) services.AddSingleton<ResponseLoggingMiddleware>();

        return services;
    }

    internal static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app, IConfiguration config)
    {
        if (config.GetValue<bool>("MiddlewareSettings:EnableLocalization")) app.UseMiddleware<LocalizationMiddleware>();
        if (config.GetValue<bool>("MiddlewareSettings:EnableHttpsLogging")) app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
        if (config.GetValue<bool>("MiddlewareSettings:EnableHttpsLogging")) app.UseMiddleware<ResponseLoggingMiddleware>();

        return app;
    }
}