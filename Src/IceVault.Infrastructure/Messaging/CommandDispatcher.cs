using IceVault.Common.Events;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;
using Newtonsoft.Json;

namespace IceVault.Infrastructure.Messaging;

internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly IEventBus _bus;

    public CommandDispatcher(IServiceProvider provider, IEventBus bus)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Dispatch(Envelope<ICommand> envelope)
    {
        var command = JsonConvert.DeserializeObject(envelope.Payload, Type.GetType(envelope.Type) ?? throw new InvalidOperationException());
        if (command == null) throw new InvalidOperationException("Command can not be null");

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

        dynamic handler = _provider.GetService(handlerType);
        if (handler == null) throw new ArgumentNullException($"Cant resolve handler for command with type ${command.GetType()}");

        var result = (Result) await handler.HandleAsync((dynamic)command, envelope);
        if (result.IsFailure)
        {
            var failures = result.Failures.ToList();
            await _bus.Publish(new CommandFailedEvent(failures), envelope);
        }
    }
}