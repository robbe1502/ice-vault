using System.Diagnostics.CodeAnalysis;
using IceVault.Common.Entities;

namespace IceVault.Application.Repositories.Entities;

public class OutboxMessage : Entity
{
    private readonly List<OutboxMessageConsumer> _consumers = new();

    [ExcludeFromCodeCoverage]
    private OutboxMessage()
    {
    }

    public OutboxMessage(string correlationId, string type, string payload, string userId, string userName)
    {
        CorrelationId = correlationId;
        Type = type;
        Payload = payload;
        UserName = userName;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string CorrelationId { get; }

    public string Type { get; }

    public string Payload { get; }
    
    public string UserName { get; }

    public DateTime CreatedAt { get; }

    public DateTime? ProcessedAt { get; private set; }

    public string Error { get; private set; }

    public IReadOnlyCollection<OutboxMessageConsumer> Consumers => _consumers;

    public void AddConsumer(OutboxMessageConsumer consumer)
    {
        var exists = _consumers.Any(el => el.Name == consumer.Name);
        if (exists) return;
        
        _consumers.Add(consumer);
    }

    public void Process()
    {
        ProcessedAt = DateTime.UtcNow;
    }

    public void SetError(string error)
    {
        Error = error;
    }
}