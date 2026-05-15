using Momentum.BuildingBlocks.Domain;

namespace Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}