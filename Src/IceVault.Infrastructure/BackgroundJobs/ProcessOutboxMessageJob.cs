using IceVault.Application.Repositories;
using IceVault.Common.Messaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace IceVault.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessageJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventDispatcher _dispatcher;
    private readonly ILogger<ProcessOutboxMessageJob> _logger;

    public ProcessOutboxMessageJob(IUnitOfWork unitOfWork, IEventDispatcher dispatcher, ILogger<ProcessOutboxMessageJob> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _unitOfWork.OutboxMessageRepository.FindAll(el => el.ProcessedAt == null, 20);
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