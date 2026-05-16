using Autofac;
using Momentum.BuildingBlocks.Infrastructure.EventBus;
using Momentum.BuildingBlocks.Infrastructure.Serialization;
using Momentum.Modules.UserAccess.Domain.Inbox;
using Newtonsoft.Json;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.EventsBus
{
    internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T>
        where T : IntegrationEvent
    {
        public async Task Handle(T @event)
        {
            using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
            {
                var context = scope.Resolve<UserAccessContext>();

                string type = @event.GetType().FullName ?? @event.GetType().Name;
                var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                }) ?? string.Empty;

                var inboxMessage = new InboxMessage(@event.Id, @event.OccurredOn, type, data);

                context.InboxMessages.Add(inboxMessage);

                await context.SaveChangesAsync();
            }
        }
    }
}
