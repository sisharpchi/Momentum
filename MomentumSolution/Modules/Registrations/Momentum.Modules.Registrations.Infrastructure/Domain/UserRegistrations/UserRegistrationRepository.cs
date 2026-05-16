using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Registrations.Domain.UserRegistrations;

namespace Momentum.Modules.Registrations.Infrastructure.Domain.UserRegistrations
{
    public class UserRegistrationRepository(RegistrationsContext context, UnitOfWork unitOfWork)
        : GenericRepository<RegistrationsContext>(context, unitOfWork), IUserRegistrationRepository, IMainRepository
    {
        public async Task AddAsync(UserRegistration userRegistration)
        {
            await Context.AddAsync(userRegistration);
        }

        public async Task<UserRegistration> GetByIdAsync(UserRegistrationId userRegistrationId)
        {
            return await Context.UserRegistrations.FirstOrDefaultAsync(x => x.Id == userRegistrationId);
        }

        public DbSet<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }
    }
}
