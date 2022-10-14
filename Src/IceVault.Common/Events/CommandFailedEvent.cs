using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Common.Events;

public class CommandFailedEvent : IEvent
{
    public CommandFailedEvent(List<Failure> failures, string userId)
    {
        Failures = failures;
        UserId = userId;
    }

    public List<Failure> Failures { get; }

    public string UserId { get; }
}