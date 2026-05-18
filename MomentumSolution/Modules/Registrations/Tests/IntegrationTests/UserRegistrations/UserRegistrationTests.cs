using Microsoft.EntityFrameworkCore;
using Momentum.Modules.Registrations.Application.UserRegistrations.GetUserRegistration;
using Momentum.Modules.Registrations.Application.UserRegistrations.RegisterNewUser;
using Momentum.Modules.Registrations.Domain.UserRegistrations.Events;
using Momentum.Modules.Registrations.IntegrationTests.SeedWork;

namespace Momentum.Modules.Registrations.IntegrationTests.UserRegistrations;

public class UserRegistrationTests : TestBase
{
    [Fact]
    public async Task RegisterNewUserCommandTest()
    {
        await using var context = await CreateContextAsync();
        var command = new RegisterNewUserCommand(
            UserRegistrationSampleData.Login,
            UserRegistrationSampleData.Password,
            UserRegistrationSampleData.Email,
            UserRegistrationSampleData.FirstName,
            UserRegistrationSampleData.LastName,
            "confirmLink");

        var registration = await AddRegistrationAsync(
            context,
            command.Login,
            command.Password,
            command.Email,
            command.FirstName,
            command.LastName,
            command.ConfirmLink);

        var newUserRegisteredNotification = registration.DomainEvents
            .OfType<NewUserRegisteredDomainEvent>()
            .Single();

        Assert.Equal(UserRegistrationSampleData.Login, newUserRegisteredNotification.Login);

        context.ChangeTracker.Clear();

        var userRegistration = await context.UserRegistrations
            .AsNoTracking()
            .Select(savedRegistration => new UserRegistrationDto
            {
                Id = savedRegistration.Id.Value,
                Login = EF.Property<string>(savedRegistration, "_login"),
                Email = EF.Property<string>(savedRegistration, "_email"),
                FirstName = EF.Property<string>(savedRegistration, "_firstName"),
                LastName = EF.Property<string>(savedRegistration, "_lastName"),
                Name = EF.Property<string>(savedRegistration, "_name"),
                Password = EF.Property<string>(savedRegistration, "_password"),
                StatusCode = EF.Property<string>(EF.Property<object>(savedRegistration, "_status"), "Value"),
            })
            .SingleAsync(savedRegistration => savedRegistration.Id == registration.Id.Value);

        Assert.Equal(UserRegistrationSampleData.Email, userRegistration.Email);
        Assert.Equal(UserRegistrationSampleData.Login, userRegistration.Login);
        Assert.Equal(UserRegistrationSampleData.FirstName, userRegistration.FirstName);
        Assert.Equal(UserRegistrationSampleData.LastName, userRegistration.LastName);
    }
}
