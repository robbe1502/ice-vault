using IceVault.Common.Events.Authentication;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IEventBus _bus;

    public RegisterCommandHandler(IEventBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task<Result> HandleAsync(RegisterCommand command, Envelope<ICommand> envelope)
    {
        var @event = new RegisteredEvent($"{command.FirstName} {command.LastName}", command.Email, command.Locale);
        await _bus.Publish(@event, envelope);
        
        return Result.Success();
    }
}