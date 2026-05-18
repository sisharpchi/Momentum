using Autofac;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Momentum.Modules.Habits.Application.Configuration.Commands;

namespace Momentum.Modules.Habits.Infrastructure.Configuration.Processing;

internal class ProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainNotificationsMapper>()
            .As<IDomainNotificationsMapper>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>()
            .AsSelf()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));
    }
}
