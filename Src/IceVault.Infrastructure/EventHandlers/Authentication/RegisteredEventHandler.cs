using IceVault.Common.Events.Authentication;
using IceVault.Common.Mails;
using IceVault.Common.Messaging;
using IceVault.Infrastructure.Mails.Makers;

namespace IceVault.Infrastructure.EventHandlers.Authentication;

public class RegisteredEventHandler : IEventHandler<RegisteredEvent>
{
    private readonly IMailService _service;

    public RegisteredEventHandler(IMailService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
    
    public async Task HandleAsync(RegisteredEvent @event)
    {
        await _service.Send(new RegisterEmailMaker(@event.Email));
    }
}