using Autofac;
using Microsoft.Extensions.Logging;
using Momentum.BuildingBlocks.Application;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.Habits.Infrastructure.Configuration.DataAccess;
using Momentum.Modules.Habits.Infrastructure.Configuration.Mediation;
using Momentum.Modules.Habits.Infrastructure.Configuration.Processing;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Momentum.Modules.Habits.Infrastructure.Configuration;

public static class HabitsStartup
{
    public static void Initialize(
        string connectionString,
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger)
    {
        var containerBuilder = new ContainerBuilder();
        var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger.ForContext("Module", "Habits"));

        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new ProcessingModule());
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterInstance(executionContextAccessor);

        var container = containerBuilder.Build();
        HabitsCompositionRoot.SetContainer(container);
    }
}
