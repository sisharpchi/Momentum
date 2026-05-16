using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.BuildingBlocks.Infrastructure.Emails;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Momentum.Modules.UserAccess.Application.Emails;
using Momentum.Modules.UserAccess.Domain.Inbox;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Inbox;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure.InternalCommands;
using Momentum.Modules.UserAccess.Infrastructure.Outbox;
using AuthenticateUserDto = Momentum.Modules.UserAccess.Application.Authentication.Authenticate.UserDto;
using GetUserDto = Momentum.Modules.UserAccess.Application.Users.GetUser.UserDto;

namespace Momentum.Modules.UserAccess.Infrastructure;

public class UserAccessContext(DbContextOptions options, ILoggerFactory loggerFactory) : DbContext(options), IEmailsDbContext
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public DbSet<User> Users { get; set; }

    public DbSet<Email> Emails { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public DbSet<InboxMessage> InboxMessages { get; set; }

    public DbSet<InternalCommand> InternalCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());

        modelBuilder.Entity<GetUserDto>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("v_Users", "users");
        });

        modelBuilder.Entity<AuthenticateUserDto>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("v_Users", "users");
            builder.Ignore(user => user.Claims);
        });

        modelBuilder.Entity<UserPermissionDto>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("v_UserPermissions", "users");
            builder.Property(permission => permission.Code).HasColumnName("PermissionCode");
        });

        modelBuilder.Entity<EmailDto>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("Emails", "app");
        });
    }
}
