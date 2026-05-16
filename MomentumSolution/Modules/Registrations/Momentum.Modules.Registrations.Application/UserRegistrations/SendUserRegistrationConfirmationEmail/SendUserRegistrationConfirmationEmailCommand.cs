using Momentum.Modules.Registrations.Application.Configuration.Commands;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Newtonsoft.Json;

namespace Momentum.Modules.Registrations.Application.UserRegistrations.SendUserRegistrationConfirmationEmail
{
    public class SendUserRegistrationConfirmationEmailCommand : InternalCommandBase
    {
        [JsonConstructor]
        public SendUserRegistrationConfirmationEmailCommand(
            Guid id,
            UserRegistrationId userRegistrationId,
            string email,
            string confirmLink)
            : base(id)
        {
            UserRegistrationId = userRegistrationId;
            Email = email;
            ConfirmLink = confirmLink;
        }

        internal UserRegistrationId UserRegistrationId { get; }

        internal string Email { get; }

        internal string ConfirmLink { get; }
    }
}