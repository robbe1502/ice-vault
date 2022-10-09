using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IceVault.IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IceVault.IdentityProvider;

public static class DatabaseSeeder
{
    public static void Seed(IApplicationBuilder app)
    {
        var factory = app.ApplicationServices.GetService<IServiceScopeFactory>();
        if (factory == null) throw new Exception($"IServiceScopeFactory could not be resolved from container");

        using var scope = factory.CreateScope();

        var provider = scope.ServiceProvider;
        
        var persistedGrantDbContext = provider.GetRequiredService<PersistedGrantDbContext>();
        persistedGrantDbContext.Database.Migrate();

        var configurationDbContext = provider.GetRequiredService<ConfigurationDbContext>();
        configurationDbContext.Database.Migrate();

        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in Config.Clients)
            {
                configurationDbContext.Clients.Add(client.ToEntity());
            }
        }

        if (!configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                configurationDbContext.IdentityResources.Add(resource.ToEntity());
            }
        }

        if (!configurationDbContext.ApiScopes.Any())
        {
            foreach (var apiScope in Config.ApiScopes)
            {
                configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
            }
        }

        configurationDbContext.SaveChanges();


        var applicationDbContext = provider.GetRequiredService<ApplicationDbContext>();
        applicationDbContext.Database.Migrate();

        var manager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        if (!applicationDbContext.Users.Any())
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Robbe",
                LastName = "Van Bael",
                Email = "robbe.van.bael@hotmail.com",
                UserName = "robbe.van.bael@hotmail.com",
                Language = "en_GB",
                TimeZone = "Europe/Brussels",
                Currency = "EUR"
            };

            var identity = manager.CreateAsync(user, "0py$4GuMK").Result;
            if (!identity.Succeeded) throw new Exception($"Failed to seed the database with the correct user information");
        }

        applicationDbContext.SaveChanges();

    }
}