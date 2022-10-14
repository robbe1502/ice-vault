using IceVault.Application;
using IceVault.Application.Notifications.Repositories;
using IceVault.Common.Messaging;
using IceVault.Persistence.Read.Notifications.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace IceVault.Persistence.Read;

public static class DependencyInjection
{
    public static void AddReadPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Persistence:Read:ConnectionString"];
        var name = configuration["Persistence:Read:Name"];

        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped(_ => new MongoClient(connectionString).GetDatabase(name));

        services.AddHandlers(typeof(DependencyInjection).Assembly, typeof(IEventHandler<>));
    }
}