﻿using IceVault.Common.Identity;
using IceVault.Common.Messaging;
using IceVault.Infrastructure.Identity;
using IceVault.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace IceVault.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();

        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        services.AddScoped<IIdentityProvider, IdentityProvider>();
    }
}