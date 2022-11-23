﻿namespace IceVault.Application.SystemErrors.Entities;

public class SystemError
{
    private SystemError()
    {
    }

    public SystemError(string correlationId, string payload, string eventType, string user, DateTime occuredAt)
    {
        CorrelationId = correlationId;
        Payload = payload;
        EventType = eventType;
        User = user;
        OccuredAt = occuredAt;
    }
    
    public long Id { get; private set; }
    
    public string User { get; private set; }
    
    public DateTime OccuredAt { get; private set; }
    
    public string CorrelationId { get; private set; }
    
    public string Payload { get; private set; }
    
    public string EventType { get; private set; }
}