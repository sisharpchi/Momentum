using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.UserAccess.UnitTests.SeedWork;

public abstract class TestBase
{
    protected static T AssertPublishedDomainEvent<T>(Entity aggregate)
        where T : IDomainEvent
    {
        return DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().Single();
    }
}
