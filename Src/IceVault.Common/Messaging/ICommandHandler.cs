using IceVault.Common.ExceptionHandling;

namespace IceVault.Common.Messaging;

public interface ICommandHandler<T> where T : class, ICommand
{
    Task HandleAsync(Envelope<T> envelope);
}
