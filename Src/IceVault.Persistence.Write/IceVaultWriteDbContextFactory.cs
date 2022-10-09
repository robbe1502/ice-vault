using IceVault.Common.Settings;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IceVault.Persistence.Write;

public class IceVaultWriteDbContextFactory : IDesignTimeDbContextFactory<IceVaultWriteDbContext>
{
    public IceVaultWriteDbContext CreateDbContext(string[] args)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "../IceVault.WebApi");
        var configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", false).Build();

        var connectionString = configuration["Persistence:Write:ConnectionString"];
        var isLogsEnabled = bool.Parse(configuration["Persistence:Write:IsLogsEnabled"]);

        var options = Options.Create<PersistenceSetting>(new PersistenceSetting() { Write = new Common.Settings.Write()
        {
            ConnectionString = connectionString, 
            IsLogsEnabled = isLogsEnabled
        }});

        return new IceVaultWriteDbContext(options);
    }
}