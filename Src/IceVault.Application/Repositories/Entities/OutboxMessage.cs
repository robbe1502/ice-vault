using IceVault.Common.Entities;

namespace IceVault.Application.Repositories.Entities;

public class OutboxMessage : Entity
{
    private readonly List<OutboxMessageConsumer> _consumers = new();

    private OutboxMessage()
    {
    }

    public OutboxMessage(string correlationId, string type, string payload, string userId)
    {
        CorrelationId = correlationId;
        Type = type;
        Payload = payload;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string CorrelationId { get; private set; }

    public string Type { get; private set; }

    public string Payload { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public DateTime? ProcessedAt { get; private set; }

    public string Error { get; private set; }

    public IReadOnlyCollection<OutboxMessageConsumer> Consumers => _consumers;

    public void AddConsumer(OutboxMessageConsumer consumer)
    {
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