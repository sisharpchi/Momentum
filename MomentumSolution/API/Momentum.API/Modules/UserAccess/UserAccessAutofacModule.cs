using Autofac;
using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Infrastructure;

namespace Momentum.API.Modules.UserAccess
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