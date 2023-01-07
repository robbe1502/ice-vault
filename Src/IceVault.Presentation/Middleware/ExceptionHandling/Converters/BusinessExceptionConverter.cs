using System.Net;
using System.Reflection;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.ExceptionHandling.Converters;
using IceVault.Common.ExceptionHandling.Models;

namespace IceVault.Presentation.Middleware.ExceptionHandling.Converters;

public class BusinessExceptionConverter : IExceptionConverter
{
    public bool CanConvert(Exception exception)
    {
        return exception is BusinessException;
    }

    public Error Convert(Exception exception)
    {
        var businessException = (BusinessException) exception;
        return new Error((int) HttpStatusCode.Conflict, businessException.Failures);
    }
}