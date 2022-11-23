using IceVault.Application;
using IceVault.Application.Repositories;
using IceVault.Application.SystemErrors.Entities;
using IceVault.Application.SystemErrors.Repositories;
using IceVault.Common.Messaging;
using IceVault.Persistence.Read.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Persistence.Read;

public static class DependencyInjection
{
    public static void AddReadPersistence(this IServiceCollection services)
    {
        services.AddScoped<ISystemErrorRepository, SystemErrorRepository>();
        services.AddDbContext<IceVaultReadDbContext>();
        
        services.AddHandlers(typeof(DependencyInjection).Assembly, typeof(IEventHandler<>));
    }
}