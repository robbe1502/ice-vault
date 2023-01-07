using IceVault.Application.SystemErrors.Entities;
using IceVault.Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IceVault.Persistence.Read;

public class IceVaultReadDbContext : DbContext
{
    private readonly PersistenceSetting _settings;
    
    public IceVaultReadDbContext(IOptions<PersistenceSetting> options)
    {
        _settings = options.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_settings.Read.ConnectionString, el => el.UseNodaTime());
        optionsBuilder.EnableDetailedErrors(_settings.Read.IsLogsEnabled);
        optionsBuilder.EnableSensitiveDataLogging(_settings.Read.IsLogsEnabled);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IceVaultReadDbContext).Assembly);
    }
    
    public DbSet<SystemError> SystemErrors { get; set; }
}