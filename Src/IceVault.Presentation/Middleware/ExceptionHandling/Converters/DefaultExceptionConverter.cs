using System.Net;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.ExceptionHandling.Converters;
using IceVault.Common.ExceptionHandling.Models;

namespace IceVault.Presentation.Middleware.ExceptionHandling.Converters;

public class DefaultExceptionConverter : IExceptionConverter
{
    public bool CanConvert(Exception exception)
    {
        return true;
    }

    public Error Convert(Exception exception)
    {
        var failures = new List<Failure> { FailureConstant.SomethingWentWrong };
        return new Error((int) HttpStatusCode.InternalServerError, failures);
    }
}