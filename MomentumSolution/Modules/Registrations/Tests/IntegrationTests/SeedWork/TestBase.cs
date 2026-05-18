using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Momentum.BuildingBlocks.Application.Emails;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.Infrastructure;
using Momentum.Modules.Registrations.Infrastructure.Domain.UserRegistrations;
using NSubstitute;

namespace Momentum.Modules.Registrations.IntegrationTests.SeedWork;

public abstract class TestBase
{
    protected IEmailSender EmailSender { get; } = Substitute.For<IEmailSender>();

    protected static async Task<RegistrationsContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<RegistrationsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new RegistrationsContext(options, NullLoggerFactory.Instance);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    protected static UserRegistrationRepository CreateRepository(RegistrationsContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        return new UserRegistrationRepository(context, unitOfWork);
    }

    protected static async Task CommitAsync(RegistrationsContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        await unitOfWork.CommitAsync(CancellationToken.None);
    }

    protected static async Task<UserRegistration> AddRegistrationAsync(
        RegistrationsContext context,
        string login,
        string password,
        string email,
        string firstName,
        string lastName,
        string confirmLink)
    {
        var repository = CreateRepository(context);
        var registration = UserRegistration.RegisterNewUser(
            login,
            password,
            email,
            firstName,
            lastName,
            Substitute.For<IUsersCounter>(),
            confirmLink);

        await repository.AddAsync(registration);
        await CommitAsync(context);

        return registration;
    }

    protected static T GetLastOutboxMessage<T>(RegistrationsContext context)
        where T : class, MediatR.INotification
    {
        var messages = OutboxMessagesHelper.GetOutboxMessages(context);
        var lastMessage = messages.Last();
        return OutboxMessagesHelper.Deserialize<T>(lastMessage);
    }
}
