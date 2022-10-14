using IceVault.Common.Events.Authentication;
using IceVault.Common.Messaging;

namespace IceVault.Infrastructure.Mails.Authentication;

public class RegisteredEventHandler : IEventHandler<RegisteredEvent>
{
    public Task HandleAsync(RegisteredEvent @event)
    {
        // TODO: Send welcome email here
        return Task.CompletedTask;
    }
}