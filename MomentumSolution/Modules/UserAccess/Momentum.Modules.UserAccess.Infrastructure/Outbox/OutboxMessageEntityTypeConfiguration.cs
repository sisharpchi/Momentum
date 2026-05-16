using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.BuildingBlocks.Application.Outbox;

namespace Momentum.Modules.UserAccess.Infrastructure.Outbox;

internal class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages", "users");

        builder.HasKey(message => message.Id);
        builder.Property(message => message.Id).ValueGeneratedNever();
    }
}
