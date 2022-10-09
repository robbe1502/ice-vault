namespace IceVault.Common.Messaging;

public interface ICommandBus
{
    Task Send(ICommand command);
}
