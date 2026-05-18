using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Registrations.UnitTests.SeedWork;

public abstract class TestBase
{
    protected static T AssertPublishedDomainEvent<T>(Entity aggregate)
        where T : IDomainEvent
    {
        return DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().Single();
    }

    protected static void AssertBrokenRule<TRule>(Action action)
        where TRule : class, IBusinessRule
    {
        var exception = Assert.Throws<BusinessRuleValidationException>(action);
        Assert.IsType<TRule>(exception.BrokenRule);
    }
}
