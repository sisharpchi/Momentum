using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;
using Momentum.Modules.Habits.Infrastructure.Domain.HabitGroups;
using Momentum.Modules.Habits.Infrastructure.Domain.Habits;
using Momentum.Modules.Habits.Infrastructure.InternalCommands;
using Momentum.Modules.Habits.Infrastructure.Outbox;

namespace Momentum.Modules.Habits.Infrastructure;

public class HabitsContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Habit> Habits { get; set; }

    public DbSet<HabitGroup> HabitGroups { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HabitEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new HabitGroupEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
