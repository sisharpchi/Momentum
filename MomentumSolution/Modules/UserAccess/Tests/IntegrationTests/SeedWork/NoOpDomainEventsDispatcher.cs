using Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching;

namespace Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

internal class NoOpDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchEventsAsync() => Task.CompletedTask;
}
