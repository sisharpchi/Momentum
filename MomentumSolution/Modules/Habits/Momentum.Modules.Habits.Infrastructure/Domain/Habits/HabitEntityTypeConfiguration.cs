using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Infrastructure.Domain.Habits;

internal class HabitEntityTypeConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.ToTable("Habits", "habits");

        builder.HasKey(habit => habit.Id);
        builder.Property(habit => habit.Id)
            .HasConversion(id => id.Value, value => new HabitId(value))
            .ValueGeneratedNever();
        builder.Property(habit => habit.UserId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();
        builder.Property(habit => habit.GroupId)
            .HasConversion(id => id.Value, value => new HabitGroupId(value))
            .IsRequired();

        builder.Property<string>("_title").HasColumnName("Title").IsRequired();
        builder.Property<string?>("_description").HasColumnName("Description");
        builder.Property<int>("_sortOrder").HasColumnName("SortOrder");

        builder.OwnsOne(habit => habit.Icon, icon =>
        {
            icon.Property(x => x.Type).HasColumnName("IconType").IsRequired();
            icon.Property(x => x.Value).HasColumnName("IconValue").IsRequired();
        });

        builder.OwnsOne(habit => habit.Color, color =>
        {
            color.Property(x => x.Value).HasColumnName("Color").IsRequired();
        });

        builder.OwnsOne(habit => habit.Type, type =>
        {
            type.Property(x => x.Value).HasColumnName("Type").IsRequired();
        });

        builder.OwnsOne(habit => habit.Status, status =>
        {
            status.Property(x => x.Value).HasColumnName("Status").IsRequired();
        });

        builder.OwnsOne(habit => habit.Target, target =>
        {
            target.Property(x => x.Value).HasColumnName("TargetValue");
            target.Property(x => x.Unit).HasColumnName("TargetUnit");
        });

        builder.OwnsMany(
            habit => habit.ScheduleVersions,
            scheduleVersion =>
            {
                scheduleVersion.ToTable("HabitScheduleVersions", "habits");
                scheduleVersion.WithOwner().HasForeignKey("HabitId");
                scheduleVersion.Property<Guid>("Id");
                scheduleVersion.HasKey("Id");
                scheduleVersion.Property(version => version.EffectiveFrom).IsRequired();
                scheduleVersion.Property(version => version.EffectiveTo);

                scheduleVersion.OwnsOne(version => version.Schedule, schedule =>
                {
                    schedule.OwnsOne(x => x.Frequency, frequency =>
                    {
                        frequency.Property(x => x.Value).HasColumnName("Frequency").IsRequired();
                    });
                    schedule.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
                    schedule.Property(x => x.EndDate).HasColumnName("EndDate");
                    schedule.Property(x => x.Timezone).HasColumnName("Timezone").IsRequired();
                    schedule.Ignore(x => x.DaysOfWeek);
                });
            });
    }
}
