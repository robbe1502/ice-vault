using IceVault.Common.Messaging;
using IceVault.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace IceVault.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly IceVaultWriteDbContext _context;
    private readonly IEventDispatcher _dispatcher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(IceVaultWriteDbContext context, IEventDispatcher dispatcher, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _context.OutboxMessages.Where(el => el.ProcessedAt == null).Take(20).ToListAsync(context.CancellationToken);
        foreach (var message in messages)
        {
            try
            {
                var @event = JsonConvert.DeserializeObject(message.Payload, Type.GetType(message.Type)!);
                await _dispatcher.Dispatch(@event as IEvent, message.CorrelationId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong processing event - {correlationId} - {id}", message.CorrelationId, message.Id);
            }
        }
    }
}