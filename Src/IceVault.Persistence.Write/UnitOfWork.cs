using IceVault.Application.Repositories;
using IceVault.Persistence.Write.Repositories;

namespace IceVault.Persistence.Write;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IceVaultWriteDbContext _context;
    private IOutboxMessageRepository _outboxMessageRepository;

    public UnitOfWork(IceVaultWriteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IOutboxMessageRepository OutboxMessageRepository => _outboxMessageRepository ??= new OutboxMessageRepository(_context);

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }
}