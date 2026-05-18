using System.Reflection;
using MediatR;
using Momentum.Modules.Registrations.Application.Contracts;
using Momentum.Modules.Registrations.Infrastructure;
using Momentum.Modules.Registrations.Infrastructure.Configuration.Processing.Outbox;
using Newtonsoft.Json;

namespace Momentum.Modules.Registrations.IntegrationTests.SeedWork;

public static class OutboxMessagesHelper
{
    public static List<OutboxMessageDto> GetOutboxMessages(RegistrationsContext context)
    {
        return context.OutboxMessages
            .OrderBy(message => message.OccurredOn)
            .Select(message => new OutboxMessageDto
            {
                Id = message.Id,
                Type = message.Type,
                Data = message.Data,
            })
            .ToList();
    }

    public static T Deserialize<T>(OutboxMessageDto message)
        where T : class, INotification
    {
        var type = Assembly.GetAssembly(typeof(CommandBase))?.GetType(typeof(T).FullName!);
        return (JsonConvert.DeserializeObject(message.Data, type!) as T)!;
    }
}
