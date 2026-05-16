using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Momentum.Modules.UserAccess.Domain.Inbox;
using MediatR;
using Newtonsoft.Json;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    internal class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
    {
        private readonly IMediator _mediator;
        private readonly IMainRepository _mainRepository;

        public ProcessInboxCommandHandler(IMediator mediator, IMainRepository mainRepository)
        {
            _mediator = mediator;
            _mainRepository = mainRepository;
        }

        public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
        {
            var messages = await _mainRepository.Query<InboxMessage>()
                .Where(message => message.ProcessedDate == null)
                .OrderBy(message => message.OccurredOn)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name ?? string.Empty));

                Type? type = messageAssembly?.GetType(message.Type);
                if (type == null)
                {
                    continue;
                }

                var request = JsonConvert.DeserializeObject(message.Data, type);

                try
                {
                    if (request is INotification notification)
                    {
                        await _mediator.Publish(notification, cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                message.MarkAsProcessed(DateTime.UtcNow);
            }

            await _mainRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
