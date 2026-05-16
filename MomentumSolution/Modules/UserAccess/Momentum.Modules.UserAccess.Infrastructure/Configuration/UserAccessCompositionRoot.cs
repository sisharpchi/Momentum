using Autofac;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration;

internal static class UserAccessCompositionRoot
{
    private static IContainer _container = default!;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    internal static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}
