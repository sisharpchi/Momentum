using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Domain.Persistence;

namespace Momentum.BuildingBlocks.Infrastructure.Persistence;

public interface IMainRepository : IRepository
{
    DbSet<T> Set<T>() where T : class;
}
