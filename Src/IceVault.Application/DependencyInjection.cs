using System.Reflection;
using FluentValidation;
using IceVault.Common.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services) 
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddHandlers(assembly, typeof(ICommandHandler<>));
        services.AddHandlers(assembly, typeof(IQueryHandler<,>));
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly, Type type)
    {
        var handlers = assembly.GetExportedTypes().Where(el => el.IsClass && el.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type)).ToList();
        foreach (var handler in handlers)
        {
            var handlerType = handler.GetInterfaces().Single(el => el.IsGenericType && el.GetGenericTypeDefinition() == type);
            services.AddScoped(handlerType, handler);
        }

        return services;
    }
}
