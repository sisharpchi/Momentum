using System.Reflection;
using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(IHabitsModule).Assembly;
}
