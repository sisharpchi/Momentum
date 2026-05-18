using Autofac;

namespace Momentum.Modules.Habits.Infrastructure.Configuration;

internal static class HabitsCompositionRoot
{
    private static IContainer _container = null!;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    internal static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}
