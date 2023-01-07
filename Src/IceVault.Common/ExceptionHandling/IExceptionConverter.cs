using IceVault.Common.ExceptionHandling.Models;

namespace IceVault.Common.ExceptionHandling.Converters;

public interface IExceptionConverter
{
    bool CanConvert(Exception exception);

    Error Convert(Exception exception);
}