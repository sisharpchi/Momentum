using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.Habits.Application.Configuration.Commands;

namespace Momentum.Modules.Habits.Infrastructure.Configuration.Mediation;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var mediatRServices = new ServiceCollection();
        mediatRServices.AddLogging();
        mediatRServices.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(IMediator).Assembly));
        builder.Populate(mediatRServices);

        builder.RegisterType<ServiceProviderWrapper>()
            .As<IServiceProvider>()
            .InstancePerDependency()
            .IfNotRegistered(typeof(IServiceProvider));

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        var mediatorOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>),
            typeof(IRequestPreProcessor<>),
            typeof(IRequestHandler<>),
            typeof(IRequestPostProcessor<,>),
            typeof(IRequestExceptionHandler<,,>),
            typeof(IRequestExceptionAction<,>),
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
        };

        builder.RegisterSource(new ScopedContravariantRegistrationSource(mediatorOpenTypes));
        foreach (var mediatorOpenType in mediatorOpenTypes)
        {
            builder.RegisterAssemblyTypes(Assemblies.Application, ThisAssembly)
                .AsClosedTypesOf(mediatorOpenType)
                .AsImplementedInterfaces();
        }

        builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    }

    private class ScopedContravariantRegistrationSource : IRegistrationSource
    {
        private readonly ContravariantRegistrationSource _source = new();
        private readonly List<Type> _types = [];

        public ScopedContravariantRegistrationSource(params Type[] types)
        {
            if (!types.All(type => type.IsGenericTypeDefinition))
            {
                throw new ArgumentException("Supplied types should be generic type definitions");
            }

            _types.AddRange(types);
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var components = _source.RegistrationsFor(service, registrationAccessor);
            foreach (var component in components)
            {
                var definitions = component.Target.Services
                    .OfType<TypedService>()
                    .Select(typedService => typedService.ServiceType.GetGenericTypeDefinition());

                if (definitions.Any(_types.Contains))
                {
                    yield return component;
                }
            }
        }
    }
}
