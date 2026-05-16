using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Registrations.Domain.UserRegistrations
{
    public class UserRegistrationId : TypedIdValueBase
    {
        public UserRegistrationId(Guid value)
            : base(value)
        {
        }
    }
}