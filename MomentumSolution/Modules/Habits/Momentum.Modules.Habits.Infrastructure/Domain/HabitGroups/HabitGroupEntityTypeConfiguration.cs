using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Infrastructure.Domain.HabitGroups;

internal class HabitGroupEntityTypeConfiguration : IEntityTypeConfiguration<HabitGroup>
{
    public void Configure(EntityTypeBuilder<HabitGroup> builder)
    {
        builder.ToTable("HabitGroups", "habits");

        builder.HasKey(group => group.Id);
        builder.Property(group => group.Id)
            .HasConversion(id => id.Value, value => new HabitGroupId(value))
            .ValueGeneratedNever();
        builder.Property(group => group.UserId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.Property<string>("_name").HasColumnName("Name").IsRequired();
        builder.Property<int>("_sortOrder").HasColumnName("SortOrder");
        builder.Property<bool>("_isArchived").HasColumnName("IsArchived");

        builder.OwnsOne(group => group.Icon, icon =>
        {
            icon.Property(x => x.Type).HasColumnName("IconType").IsRequired();
            icon.Property(x => x.Value).HasColumnName("IconValue").IsRequired();
        });

        builder.OwnsOne(group => group.Color, color =>
        {
            color.Property(x => x.Value).HasColumnName("Color").IsRequired();
        });
    }
}
