namespace IceVault.Application.Repositories;

public interface IUnitOfWork
{
    IOutboxMessageRepository OutboxMessageRepository { get; }
    
    Task SaveChangesAsync(CancellationToken token = default);
}