using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.Modules.UserAccess.Domain.Inbox;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Inbox;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure.InternalCommands;
using Momentum.Modules.UserAccess.Infrastructure.Outbox;

namespace Momentum.Modules.UserAccess.Infrastructure;

public class UserAccessContext(DbContextOptions options, ILoggerFactory loggerFactory) : DbContext(options)
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public DbSet<User> Users { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<InboxMessage> InboxMessages { get; set; }

    public DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
