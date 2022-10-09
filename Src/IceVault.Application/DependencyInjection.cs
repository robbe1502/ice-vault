using System.Reflection;
using IceVault.Common.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services) 
    {
        var assembly = typeof(DependencyInjection).Assembly;

        AddHandlers(services, assembly, typeof(ICommandHandler<>));
        AddHandlers(services, assembly, typeof(IQueryHandler<,>));
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
