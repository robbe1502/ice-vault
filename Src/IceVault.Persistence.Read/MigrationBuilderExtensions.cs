using Microsoft.EntityFrameworkCore.Migrations;

namespace IceVault.Persistence.Read;

public static class MigrationBuilderExtensions
{
    public static string ReadSql(this MigrationBuilder builder, string migrationName)
    {
        var assembly = typeof(IceVaultReadDbContext).Assembly;
        var resource = $"IceVault.Persistence.Read.Migrations.{migrationName}.sql";

        using var stream = assembly.GetManifestResourceStream(resource);
        if (stream == null) throw new Exception($"Could not find resource {resource}");
        
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}