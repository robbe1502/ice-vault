using IceVault.Common.Messaging;

namespace IceVault.Infrastructure.Messaging;

internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;

    public CommandDispatcher(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async Task Dispatch(ICommand command)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

        dynamic handler = _provider.GetService(handlerType);
        if (handler == null) throw new ArgumentNullException($"Cant resolve handler for command with type ${command.GetType()}");

        await handler.HandleAsync((dynamic)command);
    }
}