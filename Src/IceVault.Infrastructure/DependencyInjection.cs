using IceVault.Common.Identity;
using IceVault.Common.Messaging;
using IceVault.Infrastructure.Identity;
using IceVault.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using IceVault.Application;

namespace IceVault.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        services.AddSingleton<IEventBus, EventBus>();
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddHandlers(typeof(DependencyInjection).Assembly, typeof(IEventHandler<>));

        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();
    }
}
