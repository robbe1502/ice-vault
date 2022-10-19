using IceVault.Application.Notifications.Entities;
using IceVault.Application.Notifications.Queries;
using IceVault.Application.Notifications.Repositories;
using IceVault.Common.Messaging;

namespace IceVault.Application.Notifications.QueryHandlers;

public class GetAllNotificationQueryHandler : IQueryHandler<GetAllNotificationQuery, List<Notification>>
{
    private readonly INotificationRepository _repository;

    public GetAllNotificationQueryHandler(INotificationRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<List<Notification>> HandleAsync(GetAllNotificationQuery query)
    {
        return await _repository.GetMostRecentNotifications(5);
    }
}