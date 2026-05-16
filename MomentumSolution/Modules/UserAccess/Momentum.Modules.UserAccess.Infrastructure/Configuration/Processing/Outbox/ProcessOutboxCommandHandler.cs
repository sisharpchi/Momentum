using Momentum.BuildingBlocks.Application.Events;
using Momentum.BuildingBlocks.Application.Outbox;
using Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand>
    {
        private readonly IMediator _mediator;

        private readonly IMainRepository _mainRepository;

        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        public ProcessOutboxCommandHandler(
            IMediator mediator,
            IMainRepository mainRepository,
            IDomainNotificationsMapper domainNotificationsMapper)
        {
            _mediator = mediator;
            _mainRepository = mainRepository;
            _domainNotificationsMapper = domainNotificationsMapper;
        }

        public async Task Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
        {
            var messagesList = await _mainRepository.Query<OutboxMessage>()
                .Where(message => message.ProcessedDate == null)
                .OrderBy(message => message.OccurredOn)
                .ToListAsync(cancellationToken);

            if (messagesList.Count > 0)
            {
                foreach (var message in messagesList)
                {
                    var type = _domainNotificationsMapper.GetType(message.Type);
                    var @event = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

                    if (@event == null)
                    {
                        continue;
                    }

                    using (LogContext.Push(new OutboxMessageContextEnricher(@event)))
                    {
                        await this._mediator.Publish(@event, cancellationToken);

                        message.ProcessedDate = DateTime.UtcNow;
                    }
                }

                await _mainRepository.UnitOfWork.CommitAsync(cancellationToken);
            }
        }

        private class OutboxMessageContextEnricher : ILogEventEnricher
        {
            private readonly IDomainEventNotification _notification;

            public OutboxMessageContextEnricher(IDomainEventNotification notification)
            {
                _notification = notification;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
            }
        }
    }
}
