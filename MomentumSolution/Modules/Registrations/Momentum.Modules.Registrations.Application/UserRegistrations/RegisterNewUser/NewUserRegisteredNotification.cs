using Momentum.BuildingBlocks.Application.Events;
using Momentum.Modules.Registrations.Domain.UserRegistrations.Events;
using Newtonsoft.Json;

namespace Momentum.Modules.Registrations.Application.UserRegistrations.RegisterNewUser
{
    public class NewUserRegisteredNotification : DomainNotificationBase<NewUserRegisteredDomainEvent>
    {
        [JsonConstructor]
        public NewUserRegisteredNotification(NewUserRegisteredDomainEvent domainEvent, Guid id)
            : base(domainEvent, id)
        {
        }
    }
}