using System.Net;
using System.Runtime.Serialization;

namespace Cep.App.Application.Common.Exceptions;

[Serializable]
public class CustomException : Exception
{
    public List<string> ErrorMessages { get; }
    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, List<string> errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }

    protected CustomException(SerializationInfo info, in StreamingContext context)
        : base(info, context)
    {
    }
}