using IceVault.Application.Notifications.Entities;
using IceVault.Application.Notifications.Queries;
using IceVault.Common.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace IceVault.Presentation.Notifications;

[Route("api/v1/notifications")]
public class NotificationController : ControllerBase
{
    private readonly IQueryDispatcher _dispatcher;

    public NotificationController(IQueryDispatcher dispatcher)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    [HttpGet]
    public async Task<List<Notification>> GetAll()
    {
        var query = new GetAllNotificationQuery();
        return await _dispatcher.Dispatch(query);
    }
}