using IceVault.Common.Settings;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IceVault.Persistence.Read;

public class IceVaultReadDbContextFactory : IDesignTimeDbContextFactory<IceVaultReadDbContext>
{
    public IceVaultReadDbContext CreateDbContext(string[] args)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "../IceVault.WebApi");
        var configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", false).Build();

        var connectionString = configuration["Persistence:Read:ConnectionString"];
        var isLogsEnabled = bool.Parse(configuration["Persistence:Read:IsLogsEnabled"]);

        var options = Options.Create(new PersistenceSetting() { Read = new Common.Settings.Read()
        {
            ConnectionString = connectionString, 
            IsLogsEnabled = isLogsEnabled
        }});

        return new IceVaultReadDbContext(options);
    }
}