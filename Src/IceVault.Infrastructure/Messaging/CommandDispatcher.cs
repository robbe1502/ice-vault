using IceVault.Common.Events;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace IceVault.Infrastructure.Messaging;

internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly IEventBus _bus;
    private readonly ILogger<CommandDispatcher> _logger;

    public CommandDispatcher(IServiceProvider provider, IEventBus bus, ILogger<CommandDispatcher> logger)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Dispatch<T>(Envelope<T> envelope) where T : ICommand
    {
        try
        {
            var commandType = Type.GetType(envelope.Type)!;
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

            dynamic handler = _provider.GetService(handlerType);
            if (handler == null) throw new ArgumentNullException($"Cant resolve handler for command with type ${commandType}");

            await handler.HandleAsync(envelope);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong handling a command - {correlationId} - {path} - {userId}", envelope.CorrelationId, envelope.RequestPath, envelope.UserId);

            var failures = exception is IceVaultException iceVaultException ? iceVaultException.Failures.ToList() : new List<Failure>() {FailureConstant.SomethingWentWrong};
            await _bus.Publish(new CommandFailedEvent(failures, envelope.UserId), envelope.CorrelationId);
        }
    }
}