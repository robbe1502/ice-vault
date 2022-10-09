using IceVault.Common.Identity;
using IceVault.Common.Messaging;
using IceVault.Infrastructure.Identity;
using IceVault.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
        AddHandlers(services, typeof(DependencyInjection).Assembly, typeof(IEventHandler<>));

        services.AddScoped<IIdentityProvider, IdentityProvider>();
    }

    public static void AddHandlers(IServiceCollection services, Assembly assembly, Type type)
    {
        var handlers = assembly.GetExportedTypes().Where(el => el.IsClass && el.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type)).ToList();
        foreach (var handler in handlers)
        {
            var handlerType = handler.GetInterfaces().Single(el => el.IsGenericType && el.GetGenericTypeDefinition() == type);
            services.AddScoped(handlerType, handler);
        }
    }
}
