using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.BuildingBlocks.Infrastructure.Persistence;

namespace Momentum.Modules.UserAccess.Infrastructure.Domain.Users
{
    public class UserRepository(UserAccessContext context, UnitOfWork unitOfWork) : GenericRepository<UserAccessContext>(context, unitOfWork), IMainRepository
    {
        public DbSet<T> Set<T>() where T : class
        {
            return Context.Set<T>();
        }
    }
}