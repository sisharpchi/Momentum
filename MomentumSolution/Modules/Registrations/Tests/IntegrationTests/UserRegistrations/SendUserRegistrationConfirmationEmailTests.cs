using System.Reflection;
using Momentum.BuildingBlocks.Application.Emails;
using Momentum.Modules.Registrations.Application.UserRegistrations.SendUserRegistrationConfirmationEmail;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.IntegrationTests.SeedWork;
using NSubstitute;

namespace Momentum.Modules.Registrations.IntegrationTests.UserRegistrations;

public class SendUserRegistrationConfirmationEmailTests : TestBase
{
    [Fact]
    public async Task SendUserRegistrationConfirmationEmailTest()
    {
        var registrationId = Guid.NewGuid();
        var confirmLink = "confirmLink/";
        var command = new SendUserRegistrationConfirmationEmailCommand(
            Guid.NewGuid(),
            new UserRegistrationId(registrationId),
            UserRegistrationSampleData.Email,
            confirmLink);

        var handlerType = typeof(SendUserRegistrationConfirmationEmailCommand).Assembly.GetType(
            "Momentum.Modules.Registrations.Application.UserRegistrations.SendUserRegistrationConfirmationEmail.SendUserRegistrationConfirmationEmailCommandHandler",
            throwOnError: true)!;
        var handler = Activator.CreateInstance(
            handlerType,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            args: [EmailSender],
            culture: null)!;
        var handle = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public)!;

        await (Task)handle.Invoke(handler, [command, CancellationToken.None])!;

        var link = $"<a href=\"{confirmLink}{registrationId}\">link</a>";
        var content = $"Welcome to MyMeetings application! Please confirm your registration using this {link}.";
        var email = new EmailMessage(
            UserRegistrationSampleData.Email,
            "MyMeetings - Please confirm your registration",
            content);

        await EmailSender.Received(1).SendEmail(email);
    }
}
