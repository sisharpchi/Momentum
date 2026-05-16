using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Users;

namespace Momentum.Modules.UserAccess.Infrastructure;

public class UserAccessContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<InternalCommand> InternalCommands { get; set; }

    private readonly ILoggerFactory _loggerFactory;

    public UserAccessContext(DbContextOptions options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
    }
}
