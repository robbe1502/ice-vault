using IceVault.Application.SystemErrors.Entities;
using IceVault.Application.SystemErrors.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceVault.Persistence.Read.Repositories;

internal class SystemErrorRepository : ISystemErrorRepository
{
    private readonly IceVaultReadDbContext _context;

    public SystemErrorRepository(IceVaultReadDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<List<SystemError>> GetSystemErrors()
    {
        return await _context.SystemErrors.ToListAsync();
    }
}