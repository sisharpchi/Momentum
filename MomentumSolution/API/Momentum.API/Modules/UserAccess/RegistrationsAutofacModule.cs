using Autofac;
using Momentum.Modules.Registrations.Application.Contracts;
using Momentum.Modules.Registrations.Infrastructure;

namespace Momentum.API.Modules.UserAccess
{
    public class RegistrationsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegistrationsModule>()
                .As<IRegistrationsModule>()
                .InstancePerLifetimeScope();
        }
    }
}
