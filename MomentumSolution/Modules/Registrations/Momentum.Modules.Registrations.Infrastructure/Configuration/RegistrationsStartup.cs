using Autofac;
using Momentum.BuildingBlocks.Application;
using Momentum.BuildingBlocks.Application.Emails;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.BuildingBlocks.Infrastructure.Emails;
using Momentum.BuildingBlocks.Infrastructure.EventBus;
using Momentum.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration;
using Momentum.Modules.Registrations.Application.UserRegistrations.RegisterNewUser;
using Momentum.Modules.Registrations.Infrastructure.Configuration.DataAccess;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Domain;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Email;
using Momentum.Modules.Registrations.Infrastructure.Configuration.EventsBus;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Logging;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Mediation;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Processing;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Processing.Outbox;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Quartz;
using Momentum.Modules.Registrations.Infrastructure.Configuration.UserAccess;
using Serilog;

namespace Momentum.Modules.Registrations.Infrastructure.Configuration
{
    public class RegistrationsStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            EmailsConfiguration emailsConfiguration,
            string textEncryptionKey,
            IEmailSender emailSender,
            IEventsBus eventsBus,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "Registrations");

            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                logger,
                emailsConfiguration,
                textEncryptionKey,
                emailSender,
                eventsBus);

            QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            EmailsConfiguration emailsConfiguration,
            string textEncryptionKey,
            IEmailSender emailSender,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Registrations")));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new UserAccessAutofacModule());

            var domainNotificationsMap = new BiDictionary<string, Type>();
            domainNotificationsMap.Add("NewUserRegisteredNotification", typeof(NewUserRegisteredNotification));
            domainNotificationsMap.Add("UserRegistrationConfirmedNotification", typeof(UserRegistrationConfirmedNotification));
            containerBuilder.RegisterModule(new OutboxModule(domainNotificationsMap));

            containerBuilder.RegisterModule(new QuartzModule());
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));
            //// containerBuilder.RegisterModule(new SecurityModule(textEncryptionKey));

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            RegistrationsCompositionRoot.SetContainer(_container);
        }
    }
}