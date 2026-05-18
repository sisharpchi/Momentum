using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.Infrastructure;
using Momentum.Modules.Registrations.Infrastructure.Domain.UserRegistrations;
using Momentum.Modules.Registrations.IntegrationTests.SeedWork;
using NSubstitute;

namespace Momentum.Modules.Registrations.IntegrationTests.UserRegistrations;

public class UserRegistrationRepositoryTests : TestBase
{
    [Fact]
    public async Task RepositoryPersistsUserRegistration()
    {
        await using var context = await CreateContextAsync();
        var repository = CreateRepository(context);
        var registration = UserRegistration.RegisterNewUser(
            "login",
            "password",
            "test@email",
            "firstName",
            "lastName",
            Substitute.For<IUsersCounter>(),
            "confirmLink");

        await repository.AddAsync(registration);
        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var saved = await repository.GetByIdAsync(registration.Id);

        Assert.Equal(registration.Id, saved.Id);
    }
}
