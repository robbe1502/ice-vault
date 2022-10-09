using IceVault.Common.Events.Authentication;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Infrastructure.Mails.Authentication;

public class RegisteredEventHandler : IEventHandler<RegisteredEvent>
{
    public Task<Result> HandleAsync(RegisteredEvent @event, Envelope<IEvent> envelope)
    {
        // TODO: Send welcome email here
        return Task.FromResult(Result.Success());
    }
}