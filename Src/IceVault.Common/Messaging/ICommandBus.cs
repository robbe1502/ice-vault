namespace IceVault.Common.Messaging;

public interface ICommandBus
{
    Task Send<T>(T command) where T : ICommand;
}
