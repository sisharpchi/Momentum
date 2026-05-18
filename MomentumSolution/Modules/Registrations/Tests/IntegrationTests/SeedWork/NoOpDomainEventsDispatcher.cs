using Momentum.BuildingBlocks.Infrastructure.DomainEventsDispatching;

namespace Momentum.Modules.Registrations.IntegrationTests.SeedWork;

internal class NoOpDomainEventsDispatcher : IDomainEventsDispatcher
{
    public Task DispatchEventsAsync() => Task.CompletedTask;
}
