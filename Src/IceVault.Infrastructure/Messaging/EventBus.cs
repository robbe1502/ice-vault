using IceVault.Common.Messaging;
using IceVault.Persistence.Write;
using IceVault.Persistence.Write.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IceVault.Infrastructure.Messaging;

internal class EventBus : IEventBus
{
    private readonly IServiceScopeFactory _factory;

    public EventBus(IServiceScopeFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task Publish(IEvent @event, Envelope<ICommand> envelope)
    {
        using var scope = _factory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IceVaultWriteDbContext>();

        var payload = JsonConvert.SerializeObject(@event, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        var type = @event.GetType().AssemblyQualifiedName;

        var message = new OutboxMessage(envelope.CorrelationId, type, payload);
        context.OutboxMessages.Add(message);

        await context.SaveChangesAsync();

        var dispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
        await dispatcher.Dispatch(Envelope.Clone(envelope, @event));
    }
}