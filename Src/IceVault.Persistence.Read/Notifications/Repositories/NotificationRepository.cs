using IceVault.Application.Notifications.Entities;
using IceVault.Application.Notifications.Repositories;
using IceVault.Common.Identity;
using MongoDB.Driver;

namespace IceVault.Persistence.Read.Notifications.Repositories;

internal class NotificationRepository : INotificationRepository
{
    private readonly ICurrentUser _user;
    private readonly IMongoCollection<Notification> _collection;

    public NotificationRepository(IMongoDatabase database, ICurrentUser user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _collection = database.GetCollection<Notification>("notifications");
    }

    public async Task<List<Notification>> GetMostRecentNotifications(int amount)
    {
        var result = _collection.Find(el => el.UserId == _user.Id && el.IsRead == false).SortByDescending(el => el.CreatedAt);
        return await result.Limit(amount).ToListAsync();
    }

    public async Task CreateAsync(Notification notification)
    {
        await _collection.InsertOneAsync(notification);
    }
}