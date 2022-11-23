using IceVault.Application.Repositories;
using IceVault.Application.Repositories.Entities;

namespace IceVault.Persistence.Write.Repositories;

public class OutboxMessageRepository : RepositoryAsync<OutboxMessage>, IOutboxMessageRepository
{
    public OutboxMessageRepository(IceVaultWriteDbContext context) : base(context)
    {
    }
}