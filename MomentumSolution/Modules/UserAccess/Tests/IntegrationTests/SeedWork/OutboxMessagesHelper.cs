using System.Reflection;
using MediatR;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.Modules.UserAccess.Application.Authentication.Authenticate;
using Momentum.Modules.UserAccess.Infrastructure;
using Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Newtonsoft.Json;

namespace Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

public static class OutboxMessagesHelper
{
    public static List<OutboxMessageDto> GetOutboxMessages(UserAccessContext context)
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

    public static T Deserialize<T>(OutboxMessage message)
        where T : class, INotification
    {
        var type = Assembly.GetAssembly(typeof(AuthenticateCommand))?.GetType(typeof(T).FullName!);
        return (JsonConvert.DeserializeObject(message.Data, type!) as T)!;
    }

    public static T Deserialize<T>(OutboxMessageDto message)
        where T : class, INotification
    {
        var type = Assembly.GetAssembly(typeof(AuthenticateCommand))?.GetType(typeof(T).FullName!);
        return (JsonConvert.DeserializeObject(message.Data, type!) as T)!;
    }
}
