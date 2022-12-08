namespace IceVault.Application.Repositories.Entities;

public class OutboxMessageConsumer
{
    public OutboxMessageConsumer(string name)
    {
        Name = name;
    }

    public long Id { get; private set; }

    public string Name { get; private set; }

    public OutboxMessage OutboxMessage { get; private set; }

    public long OutboxMessageId { get; private set; }
}