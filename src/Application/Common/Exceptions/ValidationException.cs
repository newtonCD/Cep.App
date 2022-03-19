using System.Net;
using System.Runtime.Serialization;

namespace Cep.App.Application.Common.Exceptions;

[Serializable]
public class ValidationException : CustomException
{
    public ValidationException(List<string> errors = default)
        : base("Validation Failures Occured.", errors, HttpStatusCode.BadRequest)
    {
    }

    protected ValidationException(SerializationInfo info, in StreamingContext context)
    : base(info, context)
    {
    }
}