using MediatR;

namespace Momentum.BuildingBlocks.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}