using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.Domain.UserRegistrations.Events;
using Momentum.Modules.Registrations.Domain.UserRegistrations.Rules;
using Momentum.Modules.Registrations.UnitTests.SeedWork;
using NSubstitute;

namespace Momentum.Modules.Registrations.UnitTests.UserRegistrations;

public class UserRegistrationTests : TestBase
{
    [Fact]
    public void NewUserRegistrationWithUniqueLoginIsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();

        var registration = UserRegistration.RegisterNewUser(
            "login",
            "password",
            "test@email",
            "firstName",
            "lastName",
            usersCounter,
            "confirmLink");

        var domainEvent = AssertPublishedDomainEvent<NewUserRegisteredDomainEvent>(registration);
        Assert.Equal(registration.Id, domainEvent.UserRegistrationId);
    }

    [Fact]
    public void NewUserRegistrationWithoutUniqueLoginBreaksUserLoginMustBeUniqueRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithLogin("login").Returns(1);

        AssertBrokenRule<UserLoginMustBeUniqueRule>(() =>
            UserRegistration.RegisterNewUser(
                "login",
                "password",
                "test@email",
                "firstName",
                "lastName",
                usersCounter,
                "confirmLink"));
    }

    [Fact]
    public void ConfirmingWaitingUserRegistrationIsSuccessful()
    {
        var registration = CreateRegistration();

        registration.Confirm();

        var domainEvent = AssertPublishedDomainEvent<UserRegistrationConfirmedDomainEvent>(registration);
        Assert.Equal(registration.Id, domainEvent.UserRegistrationId);
    }

    [Fact]
    public void ConfirmedUserRegistrationCannotBeConfirmedAgain()
    {
        var registration = CreateRegistration();
        registration.Confirm();

        AssertBrokenRule<UserRegistrationCannotBeConfirmedMoreThanOnceRule>(() => registration.Confirm());
    }

    [Fact]
    public void ExpiredUserRegistrationCannotBeConfirmed()
    {
        var registration = CreateRegistration();
        registration.Expire();

        AssertBrokenRule<UserRegistrationCannotBeConfirmedAfterExpirationRule>(() => registration.Confirm());
    }

    [Fact]
    public void ExpiringWaitingUserRegistrationIsSuccessful()
    {
        var registration = CreateRegistration();

        registration.Expire();

        var domainEvent = AssertPublishedDomainEvent<UserRegistrationExpiredDomainEvent>(registration);
        Assert.Equal(registration.Id, domainEvent.UserRegistrationId);
    }

    [Fact]
    public void ExpiredUserRegistrationCannotBeExpiredAgain()
    {
        var registration = CreateRegistration();
        registration.Expire();

        AssertBrokenRule<UserRegistrationCannotBeExpiredMoreThanOnceRule>(() => registration.Expire());
    }

    private static UserRegistration CreateRegistration()
    {
        return UserRegistration.RegisterNewUser(
            "login",
            "password",
            "test@email",
            "firstName",
            "lastName",
            Substitute.For<IUsersCounter>(),
            "confirmLink");
    }
}
