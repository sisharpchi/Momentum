using Autofac;
using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Infrastructure;

namespace Momentum.Modules.Registrations.Infrastructure.Configuration.UserAccess
{
    public class UserAccessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAccessModule>()
                .As<IUserAccessModule>()
                .InstancePerLifetimeScope();
        }
    }
}