using System.Collections;
using System.Reflection;
using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.UnitTests.SeedWork;

public static class DomainEventsTestHelper
{
    public static List<IDomainEvent> GetAllDomainEvents(Entity aggregate)
    {
        List<IDomainEvent> domainEvents = [];

        if (aggregate.DomainEvents != null)
        {
            domainEvents.AddRange(aggregate.DomainEvents);
        }

        var fields = aggregate.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Concat(aggregate.GetType().BaseType?.GetFields(
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public) ?? [])
            .ToArray();

        foreach (var field in fields)
        {
            if (typeof(Entity).IsAssignableFrom(field.FieldType) &&
                field.GetValue(aggregate) is Entity entity)
            {
                domainEvents.AddRange(GetAllDomainEvents(entity));
            }

            if (field.FieldType != typeof(string) &&
                typeof(IEnumerable).IsAssignableFrom(field.FieldType) &&
                field.GetValue(aggregate) is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item is Entity entityItem)
                    {
                        domainEvents.AddRange(GetAllDomainEvents(entityItem));
                    }
                }
            }
        }

        return domainEvents;
    }
}
