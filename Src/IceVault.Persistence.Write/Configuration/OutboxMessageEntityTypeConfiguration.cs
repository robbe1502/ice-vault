using IceVault.Persistence.Write.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IceVault.Persistence.Write.Configuration;

public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(el => el.Id);

        builder.Property(el => el.Type).IsRequired();
        builder.Property(el => el.Payload).IsRequired();
        builder.Property(el => el.CorrelationId).IsRequired();
        builder.Property(el => el.CreatedAt).IsRequired();

        builder.HasMany(el => el.Consumers).WithOne(el => el.OutboxMessage).HasForeignKey(el => el.OutboxMessageId).OnDelete(DeleteBehavior.Cascade);
        builder.Metadata.FindNavigation(nameof(OutboxMessage.Consumers))?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}