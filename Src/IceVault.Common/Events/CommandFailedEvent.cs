using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Common.Events;

public class CommandFailedEvent : IEvent
{
    public CommandFailedEvent(string correlationId, List<Failure> failures, string userId)
    {
        CorrelationId = correlationId;
        Failures = failures;
        UserId = userId;
    }

    public List<Failure> Failures { get; }

    public string CorrelationId { get; }
    
    public string UserId { get; }
}