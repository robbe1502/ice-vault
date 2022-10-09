using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Common.Events;

public class CommandFailedEvent : IEvent
{
    public CommandFailedEvent(List<Failure> failures)
    {
        Failures = failures;
    }

    public List<Failure> Failures { get; }
}