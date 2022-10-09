namespace IceVault.Common.Messaging;

public interface ICommandDispatcher
{
    Task Dispatch(ICommand command);
}