using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;

namespace Momentum.Modules.UserAccess.Infrastructure.InternalCommands;

internal class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("InternalCommands", "users");

        builder.HasKey(command => command.Id);
        builder.Property(command => command.Id).ValueGeneratedNever();
    }
}
