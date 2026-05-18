using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;

namespace Momentum.Modules.Habits.Infrastructure.InternalCommands;

internal class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("InternalCommands", "habits");

        builder.HasKey(command => command.Id);
        builder.Property(command => command.Id).ValueGeneratedNever();
        builder.Property(command => command.Type).IsRequired();
        builder.Property(command => command.Data).IsRequired();
        builder.Property(command => command.EnqueueDate).IsRequired();
        builder.Property(command => command.Error);
    }
}
