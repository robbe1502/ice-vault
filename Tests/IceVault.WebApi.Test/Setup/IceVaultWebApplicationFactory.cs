using System.Security.Claims;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IceVault.Common;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Results;
using IceVault.Common.Settings;
using IceVault.Persistence.Read;
using IceVault.Persistence.Write;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Xunit;

namespace IceVault.WebApi.Test.Setup;

public class IceVaultWebApplicationFactory : WebApplicationFactory<WebApiProgram>, IAsyncLifetime
{
    private readonly MsSqlTestcontainer _db;
    
    public IceVaultWebApplicationFactory()
    {
        _db = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Database = "IceVault", 
                Environments = { { "SA_PASSWORD", "LocalTestingPassword#123" } }
            })
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _db.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _db.StopAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IIdentityProvider));
            services.RemoveAll(typeof(IOptions<PersistenceSetting>));
            services.RemoveAll(typeof(JwtBearerOptions));
            
            services.AddScoped<IIdentityProvider, FakeIdentityProvider>();

            var options = Options.Create(new PersistenceSetting
            {
                Write = new Write { ConnectionString = _db.ConnectionString, IsLogsEnabled = true },
                Read = new Read { ConnectionString = _db.ConnectionString, IsLogsEnabled = true }
            });

            services.AddScoped(_ => options);

            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, config =>
            {
                var oidc = new OpenIdConnectConfiguration { Issuer = FakeJwtTokens.Issuer };
                oidc.SigningKeys.Add(FakeJwtTokens.SecurityKey);
                
                config.Configuration = oidc;
                config.SaveToken = true;
            });
            
            // Migrations
            using var scope = services.BuildServiceProvider().CreateScope();
            var provider = scope.ServiceProvider;

            var iceVaultWriteDbContext = provider.GetRequiredService<IceVaultWriteDbContext>();
            iceVaultWriteDbContext.Database.Migrate();

            var iceVaultReadDbContext = provider.GetRequiredService<IceVaultReadDbContext>();
            iceVaultReadDbContext.Database.Migrate();
        });
    }
}

internal class FakeIdentityProvider : IIdentityProvider
{
    public Task<TokenResult> GetTokenAsync(string email, string password)
    {
        var claims = new List<Claim>
        {
            new(IceVaultConstant.Claim.Id, "123"),
            new(IceVaultConstant.Claim.FullName, "John Doe"),
            new(IceVaultConstant.Claim.Email, "johndoe@hotmail.com"),
            new(IceVaultConstant.Claim.Locale, "en-US"),
            new(IceVaultConstant.Claim.TimeZone, "Europe/Brussels"),
            new(IceVaultConstant.Claim.Currency, "EUR"),
            new ("scope", "ice-vault-web-api"),
            new ("scope", "email"),
            new ("scope", "openid"),
            new ("scope", "profile"),
            new ("scope", "profile-data"),
            new ("scope", "offline_access"),
            new ("iss", FakeJwtTokens.Issuer),
            new ("client_id", "7E9C08BE-2DE9-4E85-9060-B1A1CC78559F")
        };

        var token = FakeJwtTokens.GenerateToken(claims);
        return Task.FromResult(new TokenResult(token, string.Empty, 3600));
    }

    public async Task<TokenResult> RefreshTokenAsync(string token)
    {
        return await GetTokenAsync(string.Empty, string.Empty);
    }

    public Task<UserResult> GetProfileAsync(string token)
    {
        return Task.FromResult(new UserResult
        {
            Id = "123",
            Currency = "EUR",
            Email = "johndoe@hotmail.com",
            Locale = "en-US",
            Name = "John Doe",
            TimeZone = "Europe/Brussels"
        });
    }

    public async Task<UserResult> GetUserById(string id)
    {
        return await GetProfileAsync(id);
    }

    public Task<Guid> RegisterUserAsync(string firstName, string lastName, string email, string password, string locale, string timeZone, string currency)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}