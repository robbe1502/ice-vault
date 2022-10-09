namespace IceVault.Common.Messaging;

public interface ICommandHandler<T> where T : ICommand
{
    Task HandleAsync(T command);
}
