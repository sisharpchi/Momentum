using Autofac;
using MediatR;
using RegistrationsMediatorModule = Momentum.Modules.Registrations.Infrastructure.Configuration.Mediation.MediatorModule;
using UserAccessMediatorModule = Momentum.Modules.UserAccess.Infrastructure.Configuration.Mediation.MediatorModule;

namespace Momentum.BuildingBlocks.Tests.Mediation;

public class MediatorModuleTests
{
    [Fact]
    public void UserAccess_mediator_module_registers_mediatr_services()
    {
        using var container = BuildContainer(new UserAccessMediatorModule());

        var mediator = container.Resolve<IMediator>();

        Assert.NotNull(mediator);
    }

    [Fact]
    public void Registrations_mediator_module_registers_mediatr_services()
    {
        using var container = BuildContainer(new RegistrationsMediatorModule());

        var mediator = container.Resolve<IMediator>();

        Assert.NotNull(mediator);
    }

    private static IContainer BuildContainer(Module module)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(module);
        return builder.Build();
    }
}
