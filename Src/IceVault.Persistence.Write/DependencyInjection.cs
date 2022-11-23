using IceVault.Application.Repositories;
using IceVault.Persistence.Write.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Persistence.Write;

public static class DependencyInjection
{
    public static void AddWritePersistence(this IServiceCollection services)
    {
        services.AddScoped<IceVaultWriteDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}