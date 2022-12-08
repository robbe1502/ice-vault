using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IceVault.Common.Identity;
using IceVault.Common.Identity.Models.Results;
using IceVault.Common.Settings;
using IceVault.Persistence.Read;
using IceVault.Persistence.Write;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            .WithName("ice-vault-database")
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
            
            services.AddScoped<IIdentityProvider, FakeIdentityProvider>();

            var options = Options.Create(new PersistenceSetting
            {
                Write = new Write { ConnectionString = _db.ConnectionString, IsLogsEnabled = true },
                Read = new Read { ConnectionString = _db.ConnectionString, IsLogsEnabled = true }
            });

            services.AddScoped(_ => options);
            
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
            new(IceVaultClaimConstant.Id, "123"),
            new(IceVaultClaimConstant.FullName, "John Doe"),
            new(IceVaultClaimConstant.Email, "johndoe@hotmail.com"),
            new(IceVaultClaimConstant.Locale, "en-US"),
            new(IceVaultClaimConstant.TimeZone, "Europe/Brussels"),
            new(IceVaultClaimConstant.Currency, "EUR")
        };

        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("635BA202-2529-4E57-9D88-981E8EE73C68");

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.WriteToken(handler.CreateToken(descriptor));
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
            Id = Guid.NewGuid().ToString(),
            Currency = "EUR",
            Email = "test@hotmail.com",
            Locale = "en-US",
            Name = "John Testing",
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