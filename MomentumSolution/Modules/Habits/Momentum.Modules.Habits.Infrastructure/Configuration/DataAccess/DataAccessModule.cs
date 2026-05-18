using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Infrastructure;

namespace Momentum.Modules.Habits.Infrastructure.Configuration.DataAccess;

internal class DataAccessModule : Module
{
    private readonly string _connectionString;
    private readonly ILoggerFactory _loggerFactory;

    internal DataAccessModule(string connectionString, ILoggerFactory loggerFactory)
    {
        _connectionString = connectionString;
        _loggerFactory = loggerFactory;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(_ =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<HabitsContext>();
                optionsBuilder.UseNpgsql(_connectionString);
                optionsBuilder.UseLoggerFactory(_loggerFactory);
                optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                return new HabitsContext(optionsBuilder.Options);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(HabitsContext).Assembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
