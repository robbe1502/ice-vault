using IceVault.Common.Entities;

namespace IceVault.Application.Notifications.Entities;

public class Notification : Entity
{
    public string Title { get; set; }

    public string Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }
}