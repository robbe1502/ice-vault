
namespace IceVault.Common.Messaging;

public interface ICommandDispatcher
{
    Task Dispatch(Envelope<ICommand> envelope);
}