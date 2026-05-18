using Microsoft.EntityFrameworkCore;
using Momentum.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.IntegrationTests.SeedWork;

namespace Momentum.Modules.Registrations.IntegrationTests.UserRegistrations;

public class ConfirmUserRegistrationTests : TestBase
{
    [Fact]
    public async Task ConfirmUserRegistrationTest()
    {
        await using var context = await CreateContextAsync();
        var registration = await AddRegistrationAsync(
            context,
            UserRegistrationSampleData.Login,
            UserRegistrationSampleData.Password,
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.FirstName,
            UserRegistrationSampleData.LastName,
            "confirmLink");
        var command = new ConfirmUserRegistrationCommand(registration.Id.Value);
        var repository = CreateRepository(context);

        var userRegistration = await repository.GetByIdAsync(new UserRegistrationId(command.UserRegistrationId));
        userRegistration.Confirm();
        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var statusCode = await context.UserRegistrations
            .Where(savedRegistration => savedRegistration.Id == new UserRegistrationId(command.UserRegistrationId))
            .Select(savedRegistration => EF.Property<string>(
                EF.Property<object>(savedRegistration, "_status"),
                "Value"))
            .SingleAsync();

        Assert.Equal(UserRegistrationStatus.Confirmed.Value, statusCode);
    }
}
