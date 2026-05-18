using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.BuildingBlocks.Infrastructure.Persistence;

namespace Momentum.Modules.Habits.Infrastructure.Domain.Habits;

public class HabitRepository(HabitsContext context, UnitOfWork unitOfWork) :
    GenericRepository<HabitsContext>(context, unitOfWork),
    IMainRepository
{
    public DbSet<T> Set<T>()
        where T : class
    {
        return Context.Set<T>();
    }
}
