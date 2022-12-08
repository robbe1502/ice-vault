using IceVault.Common.ExceptionHandling;
using IceVault.Common.Messaging;

namespace IceVault.Common.Events;

public class CommandFailedEvent : IEvent
{
    public CommandFailedEvent(string correlationId, List<Failure> failures, string userId, string userName)
    {
        CorrelationId = correlationId;
        Failures = failures;
        UserId = userId;
        UserName = userName;
    }

    public List<Failure> Failures { get; }

    public string CorrelationId { get; }
    
    public string UserId { get; }
    
    public string UserName { get; }
}