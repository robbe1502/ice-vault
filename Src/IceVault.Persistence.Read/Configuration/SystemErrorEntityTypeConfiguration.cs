using IceVault.Application.SystemErrors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IceVault.Persistence.Read.Configuration;

public class SystemErrorConfiguration : IEntityTypeConfiguration<SystemError>
{
    public void Configure(EntityTypeBuilder<SystemError> builder)
    {
        builder.HasKey(el => el.Id);
        builder.ToView(nameof(IceVaultReadDbContext.SystemErrors));
    }
}