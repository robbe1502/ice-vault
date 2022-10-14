namespace IceVault.Common.Messaging;

public interface IEventDispatcher
{
    Task Dispatch(IEvent @event, string correlationId);
}