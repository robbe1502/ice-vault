using IceVault.Common.ExceptionHandling;

namespace IceVault.Common.Messaging;

public interface IEventHandler<in T> where T : IEvent
{
    Task<Result> HandleAsync(T @event, Envelope<IEvent> envelope);
}