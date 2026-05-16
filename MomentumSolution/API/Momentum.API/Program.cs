using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Momentum.API.Configuration.Authorization;
using Momentum.API.Configuration.ExecutionContext;
using Momentum.API.Configuration.Extensions;
using Momentum.API.Configuration.Validation;
using Momentum.API.Modules.UserAccess;
using Momentum.BuildingBlocks.Application;
using Momentum.BuildingBlocks.Domain;
using Momentum.BuildingBlocks.Infrastructure.Emails;
using Momentum.BuildingBlocks.Infrastructure.EventBus;
using Momentum.Modules.Registrations.Infrastructure.Configuration;
using Momentum.Modules.UserAccess.Infrastructure.Configuration;
using Momentum.Modules.UserAccess.Infrastructure.Configuration.Identity;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace Momentum.API
{
    public class Program
    {
        private const string ConnectionStringName = "MeetingsConnectionString";

        public static void Main(string[] args)
        {
            var logger = ConfigureLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    containerBuilder.RegisterModule(new UserAccessAutofacModule());
                    containerBuilder.RegisterModule(new RegistrationsAutofacModule());
                });

                builder.Services.AddControllers();
                builder.Services.AddSwaggerDocumentation();
                builder.Services.ConfigureIdentityService();
                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
                builder.Services.AddProblemDetails(options =>
                {
                    options.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                    options.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
                });
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(HasPermissionAttribute.HasPermissionPolicyName, policyBuilder =>
                    {
                        policyBuilder.Requirements.Add(new HasPermissionAuthorizationRequirement());
                        policyBuilder.AddAuthenticationSchemes("Bearer");
                    });
                });
                builder.Services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();

                var app = builder.Build();

                InitializeModules(app, logger);

                app.UseMiddleware<CorrelationMiddleware>();
                app.UseSwaggerDocumentation();
                app.AddIdentityService();

                if (app.Environment.IsDevelopment())
                {
                    app.UseProblemDetails();
                }
                else
                {
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.MapControllers();

                app.Run();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception, "API host terminated unexpectedly.");
                throw;
            }
        }

        private static ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
        }

        private static void InitializeModules(WebApplication app, ILogger logger)
        {
            var container = app.Services.GetAutofacRoot();
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            var eventsBus = new InMemoryEventBusClient(logger);
            var emailsConfiguration = new EmailsConfiguration(
                app.Configuration["EmailsConfiguration:FromEmail"] ?? "no-reply@momentum.local");
            var connectionString = app.Configuration.GetConnectionString(ConnectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured.");
            var textEncryptionKey = app.Configuration["Security:TextEncryptionKey"]
                ?? "1234567890123456";

            UserAccessStartup.Initialize(
                connectionString,
                executionContextAccessor,
                logger,
                emailsConfiguration,
                textEncryptionKey,
                null!,
                eventsBus);

            RegistrationsStartup.Initialize(
                connectionString,
                executionContextAccessor,
                logger,
                emailsConfiguration,
                textEncryptionKey,
                null!,
                eventsBus);
        }
    }
}
