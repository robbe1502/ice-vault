
namespace IceVault.Common.Messaging;

public interface ICommandDispatcher
{
    Task Dispatch<T>(Envelope<T> envelope) where T : ICommand;
}