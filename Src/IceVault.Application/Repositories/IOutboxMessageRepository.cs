using IceVault.Application.Repositories.Entities;

namespace IceVault.Application.Repositories;

public interface IOutboxMessageRepository : IRepositoryAsync<OutboxMessage>
{
}