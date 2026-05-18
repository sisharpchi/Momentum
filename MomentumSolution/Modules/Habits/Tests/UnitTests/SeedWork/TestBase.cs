using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.UnitTests.SeedWork;

public abstract class TestBase
{
    protected static T AssertPublishedDomainEvent<T>(Entity aggregate)
        where T : IDomainEvent
    {
        var domainEvent = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().SingleOrDefault();

        if (domainEvent == null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} event not published");
        }

        return domainEvent;
    }

    protected static BusinessRuleValidationException AssertBrokenRule(Action test)
    {
        return Assert.Throws<BusinessRuleValidationException>(test);
    }
}
