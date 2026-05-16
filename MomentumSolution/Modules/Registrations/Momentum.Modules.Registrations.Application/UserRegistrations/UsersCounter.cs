using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Registrations.Domain.UserRegistrations;

namespace Momentum.Modules.Registrations.Application.UserRegistrations
{
    public class UsersCounter : IUsersCounter
    {
        private readonly IMainRepository _mainRepository;

        public UsersCounter(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public int CountUsersWithLogin(string login)
        {
            return _mainRepository.Query<UserRegistration>()
                .Count(userRegistration => EF.Property<string>(userRegistration, "_login") == login);
        }
    }
}
