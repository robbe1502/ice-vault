namespace IceVault.Common.Messaging;

public interface IEventBus
{
    Task Publish(IEvent @event, Envelope<ICommand> envelope);
}