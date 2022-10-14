
using IceVault.Application.Notifications.Entities;

namespace IceVault.Application.Notifications.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetMostRecentNotifications(int amount);

    Task CreateAsync(Notification notification);
}