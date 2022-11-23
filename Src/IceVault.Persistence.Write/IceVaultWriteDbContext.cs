using IceVault.Application.Repositories.Entities;
using IceVault.Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IceVault.Persistence.Write;

public class IceVaultWriteDbContext : DbContext
{
    private readonly PersistenceSetting _settings;

    public IceVaultWriteDbContext(IOptions<PersistenceSetting> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_settings.Write.ConnectionString);
        optionsBuilder.EnableDetailedErrors(_settings.Write.IsLogsEnabled);
        optionsBuilder.EnableSensitiveDataLogging(_settings.Write.IsLogsEnabled);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IceVaultWriteDbContext).Assembly);
    }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}