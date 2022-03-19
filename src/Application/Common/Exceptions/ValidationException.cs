using System.Net;

namespace Cep.App.Application.Common.Exceptions;

[Serializable]
public class ValidationException : CustomException
{
    public ValidationException(List<string> errors = default)
        : base("Validation Failures Occured.", errors, HttpStatusCode.BadRequest)
    {
    }
}