using IceVault.Application.Notifications.Entities;
using IceVault.Application.Notifications.Repositories;
using IceVault.Common.Events;
using IceVault.Common.Messaging;
using MongoDB.Bson;

namespace IceVault.Persistence.Read.Notifications.EventHandlers;

public class CommandFailedEventHandler : IEventHandler<CommandFailedEvent>
{
    private readonly INotificationRepository _repository;

    public CommandFailedEventHandler(INotificationRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(CommandFailedEvent @event)
    {
        foreach (var failure in @event.Failures)
        {
            var notification = new Notification()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = failure.Code,
                Message = failure.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                ReadAt = null
            };

            await _repository.CreateAsync(notification);
        }
    }
}