using IceVault.Common.ExceptionHandling;

namespace IceVault.Common.Messaging;

public interface ICommandHandler<T> where T : ICommand
{
    Task<Result> HandleAsync(T command, Envelope<ICommand> envelope);
}
