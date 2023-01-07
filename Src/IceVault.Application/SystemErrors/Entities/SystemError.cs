using System.Diagnostics.CodeAnalysis;
using NodaTime;

namespace IceVault.Application.SystemErrors.Entities;

public class SystemError
{
    [ExcludeFromCodeCoverage]
    private SystemError()
    {
    }

    public SystemError(string correlationId, string payload, string eventType, string user, Instant occuredAt)
    {
        CorrelationId = correlationId;
        Payload = payload;
        EventType = eventType;
        User = user;
        OccuredAt = occuredAt;
    }
    
    [ExcludeFromCodeCoverage]
    public long Id { get; private set; }
    
    public string User { get; private set; }
    
    public Instant OccuredAt { get; private set; }
    
    public string CorrelationId { get; private set; }
    
    public string Payload { get; private set; }
    
    public string EventType { get; private set; }
}