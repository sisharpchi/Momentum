using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Modules.UserAccess.Domain.Inbox;

namespace Momentum.Modules.UserAccess.Infrastructure.Domain.Inbox;

internal class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("InboxMessages", "users");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.OccurredOn).IsRequired();
        builder.Property(message => message.Type).IsRequired();
        builder.Property(message => message.Data).IsRequired();
        builder.Property(message => message.ProcessedDate);
    }
}
