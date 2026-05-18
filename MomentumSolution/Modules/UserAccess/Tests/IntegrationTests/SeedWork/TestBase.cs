using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Momentum.BuildingBlocks.Application.Emails;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure;
using Momentum.Modules.UserAccess.Infrastructure.Domain.Users;
using NSubstitute;

namespace Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

public abstract class TestBase
{
    protected IEmailSender EmailSender { get; } = Substitute.For<IEmailSender>();

    protected static async Task<UserAccessContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<UserAccessContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UserAccessContext(options, NullLoggerFactory.Instance);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    protected static UserRepository CreateRepository(UserAccessContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        return new UserRepository(context, unitOfWork);
    }

    protected static async Task CommitAsync(UserAccessContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        await unitOfWork.CommitAsync(CancellationToken.None);
    }

    protected static T GetLastOutboxMessage<T>(UserAccessContext context)
        where T : class, MediatR.INotification
    {
        var messages = OutboxMessagesHelper.GetOutboxMessages(context);
        var lastMessage = messages.Last();
        return OutboxMessagesHelper.Deserialize<T>(lastMessage);
    }

    protected static async Task<User> AddUserAsync(
        UserAccessContext context,
        Guid userId,
        string login,
        string email,
        string firstName,
        string lastName,
        string password)
    {
        var repository = CreateRepository(context);
        var user = User.CreateUser(userId, login, password, email, firstName, lastName);
        await repository.Set<User>().AddAsync(user);
        await CommitAsync(context);
        return user;
    }
}
