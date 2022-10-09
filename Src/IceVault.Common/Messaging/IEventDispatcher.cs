namespace IceVault.Common.Messaging;

public interface IEventDispatcher
{
    Task Dispatch(Envelope<IEvent> envelope);
}