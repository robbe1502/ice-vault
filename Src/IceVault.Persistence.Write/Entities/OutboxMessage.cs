namespace IceVault.Persistence.Write.Entities;

public class OutboxMessage
{
    private readonly List<OutboxMessageConsumer> _consumers = new();

    private OutboxMessage()
    {
    }

    public OutboxMessage(string correlationId, string type, string payload)
    {
        CorrelationId = correlationId;
        Type = type;
        Payload = payload;
        CreatedAt = DateTime.UtcNow;
    }

    public long Id { get; private set; }

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