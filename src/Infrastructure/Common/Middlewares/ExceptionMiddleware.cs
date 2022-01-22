using Cep.App.Application.Common.Exceptions;
using Cep.App.Application.Common.Interfaces;
using Cep.App.Application.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Serilog;
using Serilog.Context;
using System.Net;

namespace Cep.App.Infrastructure.Common.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ISerializerService _jsonSerializer;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

    public ExceptionMiddleware(
        ISerializerService jsonSerializer,
        IStringLocalizer<ExceptionMiddleware> localizer)
    {
        _jsonSerializer = jsonSerializer;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var responseModel = await ErrorResult<string>.ReturnErrorAsync(exception.Message);
            responseModel.Source = exception.TargetSite.DeclaringType.FullName;
            responseModel.Exception = exception.Message.Trim();
            responseModel.ErrorId = errorId;
            responseModel.SupportMessage = _localizer["exceptionmiddleware.supportmessage"];
            var response = context.Response;
            response.ContentType = "application/json";
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    response.StatusCode = responseModel.StatusCode = (int)e.StatusCode;
                    responseModel.Messages = e.ErrorMessages;
                    break;

                case KeyNotFoundException:
                    response.StatusCode = responseModel.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{responseModel.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            await response.WriteAsync(_jsonSerializer.Serialize(responseModel));
        }
    }
}