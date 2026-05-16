    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Momentum.BuildingBlocks.Infrastructure.Emails
{
    public class EmailEntityTypeConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.ToTable("Emails", "app");

            builder.HasKey(email => email.Id);

            builder.Property(email => email.From)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(email => email.To)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(email => email.Subject)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(email => email.Content)
                .IsRequired();

            builder.Property(email => email.Date)
                .IsRequired();
        }
    }
}
