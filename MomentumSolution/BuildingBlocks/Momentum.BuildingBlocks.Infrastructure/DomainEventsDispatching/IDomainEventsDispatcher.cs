namespace Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}