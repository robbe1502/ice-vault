using System.Net;
using FluentValidation;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.ExceptionHandling.Converters;
using IceVault.Common.ExceptionHandling.Models;

namespace IceVault.Presentation.Middleware.ExceptionHandling.Converters;

public class ValidationExceptionConverter : IExceptionConverter
{
    public bool CanConvert(Exception exception)
    {
        return exception is ValidationException;
    }

    public Error Convert(Exception exception)
    {
        var validationException = (ValidationException) exception;
        var failures = validationException.Errors.Select(el => new Failure(el.ErrorCode, el.ErrorMessage)).ToList();
        return new Error((int) HttpStatusCode.BadRequest, failures);
    }
}