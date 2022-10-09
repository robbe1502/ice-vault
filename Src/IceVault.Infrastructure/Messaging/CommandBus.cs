using IceVault.Common.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Infrastructure.Messaging;

internal class CommandBus : ICommandBus
{
    private readonly IServiceScopeFactory _factory;

    public CommandBus(IServiceScopeFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task Send(ICommand command)
    {
        using var scope = _factory.CreateScope();

        var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        await dispatcher.Dispatch(command);
    }
}
