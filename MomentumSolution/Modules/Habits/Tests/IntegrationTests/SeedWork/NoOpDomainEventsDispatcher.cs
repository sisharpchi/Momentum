using Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching;

namespace Momentum.Modules.Habits.IntegrationTests.SeedWork;

internal class NoOpDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchEventsAsync() => Task.CompletedTask;
}
