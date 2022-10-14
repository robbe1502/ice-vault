using IceVault.Common.Events.Authentication;
using IceVault.Common.Identity;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IIdentityProvider _provider;
    private readonly IEventBus _bus;

    public RegisterCommandHandler(IIdentityProvider provider, IEventBus bus)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task HandleAsync(Envelope<RegisterCommand> envelope)
    {
        var command = envelope.GetPayload();

        var id = await _provider.RegisterUserAsync(command.FirstName, command.LastName, command.Email, command.Password, command.Locale, command.TimeZone, command.Currency);

        var @event = new RegisteredEvent($"{command.FirstName} {command.LastName}", command.Email, command.Locale);
        await _bus.Publish(@event, envelope.CorrelationId);
    }
}