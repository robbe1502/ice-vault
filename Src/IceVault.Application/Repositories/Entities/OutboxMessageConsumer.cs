namespace IceVault.Application.Repositories.Entities;

public class OutboxMessageConsumer
{
    private OutboxMessageConsumer()
    {
    }

    public OutboxMessageConsumer(string name)
    {
        Name = name;
    }

    public long Id { get; set; }

    public string Name { get; set; }

    public OutboxMessage OutboxMessage { get; set; }

    public long OutboxMessageId { get; set; }
}