using IceVault.Common.Messaging;
using IceVault.Persistence.Write;
using IceVault.Persistence.Write.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;

namespace IceVault.Infrastructure.Messaging;

internal class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly IceVaultWriteDbContext _context;

    public EventDispatcher(IServiceProvider provider, IceVaultWriteDbContext context)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Dispatch(IEvent @event, string correlationId)
    {
        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        var handlers = _provider.GetServices(handlerType).ToList();

        var outboxMessage = await _context.OutboxMessages.Include(el => el.Consumers).SingleOrDefaultAsync(el => el.CorrelationId == correlationId && el.Type == @event.GetType().AssemblyQualifiedName);
        if (outboxMessage == null) throw new InvalidOperationException($"Outbox message with CorrelationId {correlationId} was not found");

        var policy = Policy.Handle<Exception>().WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(1 * attempt));
        var result = await policy.ExecuteAndCaptureAsync(async () =>
        {
            foreach (dynamic handler in handlers)
            {
                if (outboxMessage.Consumers.Any(el => el.Name == handler.GetType().Name)) continue;

                await handler.HandleAsync((dynamic)@event);

                var consumer = new OutboxMessageConsumer(handler.GetType().Name);
                outboxMessage.AddConsumer(consumer);
            }
        });


        outboxMessage.Process();
        outboxMessage.SetError(result.FinalException?.ToString());

        await _context.SaveChangesAsync();
    }
}