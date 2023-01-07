using IceVault.Application.Repositories;
using IceVault.Common.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IceVault.Infrastructure.BackgroundJobs;

public class ProcessOutboxMessageJob : BackgroundService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventDispatcher _dispatcher;
    private readonly ILogger<ProcessOutboxMessageJob> _logger;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(30);

    public ProcessOutboxMessageJob(IUnitOfWork unitOfWork, IEventDispatcher dispatcher, ILogger<ProcessOutboxMessageJob> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await _unitOfWork.OutboxMessageRepository.FindAll(el => el.ProcessedAt == null, 20, stoppingToken);
            foreach (var message in messages)
            {   
                try
                {
                    var @event = JsonConvert.DeserializeObject(message.Payload, Type.GetType(message.Type)!);
                    await _dispatcher.Dispatch(@event as IEvent, message.CorrelationId);

                    await timer.WaitForNextTickAsync(stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Something went wrong processing event - {correlationId} - {id}", message.CorrelationId, message.Id);
                }
            }
        }
    }
}