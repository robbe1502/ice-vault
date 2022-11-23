using IceVault.Application.Repositories;
using IceVault.Application.Repositories.Entities;
using IceVault.Common.Messaging;
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

    public async Task Publish(IEvent @event)
    {
        using var scope = _factory.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        var type = @event.GetType().AssemblyQualifiedName;

        var payload = JsonConvert.SerializeObject(@event, settings);

        var message = new OutboxMessage(@event.CorrelationId, type, payload, @event.UserId);
        
        await unitOfWork.OutboxMessageRepository.InsertAsync(message);
        await unitOfWork.SaveChangesAsync();
    }
}