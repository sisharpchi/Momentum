using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Infrastructure;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.DataAccess;

internal class DataAccessModule : Module
{
    private readonly string _databaseConnectionString;
    private readonly ILoggerFactory _loggerFactory;

    internal DataAccessModule(string databaseConnectionString, ILoggerFactory loggerFactory)
    {
        _databaseConnectionString = databaseConnectionString;
        _loggerFactory = loggerFactory;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserAccessContext>();
                dbContextOptionsBuilder.UseNpgsql(_databaseConnectionString);
                dbContextOptionsBuilder.UseOpenIddict();

                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new UserAccessContext(dbContextOptionsBuilder.Options, _loggerFactory);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        var infrastructureAssembly = typeof(UserAccessContext).Assembly;

        builder.RegisterAssemblyTypes(infrastructureAssembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
