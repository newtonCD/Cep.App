using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace Cep.App.Infrastructure.Common.Middlewares;

public class ResponseLoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        string responseBody;

        if (context.Request.Path.ToString().Contains("tokens"))
        {
            responseBody = "[Redacted] Contains Sensitive Information.";
        }
        else
        {
            newBody.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        }

        LogContext.PushProperty("StatusCode", context.Response.StatusCode);
        LogContext.PushProperty("ResponseTimeUTC", DateTime.UtcNow);
        Log.ForContext("ResponseHeaders", context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
       .ForContext("ResponseBody", responseBody)
       .Information("HTTP {RequestMethod} Request to {RequestPath} has Status Code {StatusCode}.", context.Request.Method, context.Request.Path, context.Response.StatusCode);
        newBody.Seek(0, SeekOrigin.Begin);

        await newBody.CopyToAsync(originalBody);
    }
}