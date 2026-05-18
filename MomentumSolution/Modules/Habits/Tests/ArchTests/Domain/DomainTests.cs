using System.Reflection;
using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.ArchTests.SeedWork;
using NetArchTest.Rules;

namespace Momentum.Modules.Habits.ArchTests.Domain;

public class DomainTests : TestBase
{
    [Fact]
    public void DomainEventShouldBeImmutable()
    {
        var types = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEventBase))
            .Or()
            .ImplementInterface(typeof(IDomainEvent))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Fact]
    public void ValueObjectShouldBeImmutable()
    {
        var types = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(ValueObject))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Fact]
    public void EntityWhichIsNotAggregateRootCannotHavePublicMembers()
    {
        var types = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .And()
            .DoNotImplementInterface(typeof(IAggregateRoot))
            .GetTypes();

        const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                          BindingFlags.Public |
                                          BindingFlags.Instance |
                                          BindingFlags.Static;

        List<Type> failingTypes = [];
        foreach (var type in types)
        {
            if (type.GetFields(bindingFlags).Any() ||
                type.GetProperties(bindingFlags).Any() ||
                type.GetMethods(bindingFlags).Any())
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void EntityCannotHaveReferenceToOtherAggregateRoot()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var aggregateRoots = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IAggregateRoot))
            .GetTypes()
            .ToList();

        const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;

        List<Type> failingTypes = [];
        foreach (var type in entityTypes)
        {
            var fields = type.GetFields(bindingFlags);
            if (fields.Any(field =>
                    aggregateRoots.Contains(field.FieldType) ||
                    field.FieldType.GenericTypeArguments.Any(aggregateRoots.Contains)))
            {
                failingTypes.Add(type);
                continue;
            }

            var properties = type.GetProperties(bindingFlags);
            if (properties.Any(property =>
                    aggregateRoots.Contains(property.PropertyType) ||
                    property.PropertyType.GenericTypeArguments.Any(aggregateRoots.Contains)))
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void EntityShouldHaveParameterlessPrivateConstructor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var hasPrivateParameterlessConstructor = constructors.Any(constructorInfo =>
                constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0);

            if (!hasPrivateParameterlessConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void DomainObjectShouldHaveOnlyPrivateConstructors()
    {
        var domainObjectTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .Or()
            .Inherit(typeof(ValueObject))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var domainObjectType in domainObjectTypes)
        {
            var constructors = domainObjectType.GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (constructors.Any(constructorInfo => !constructorInfo.IsPrivate))
            {
                failingTypes.Add(domainObjectType);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void ValueObjectShouldHavePrivateConstructorWithParametersForItsState()
    {
        var valueObjects = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(ValueObject))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var entityType in valueObjects)
        {
            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                              BindingFlags.Public |
                                              BindingFlags.Instance;
            var names = entityType.GetFields(bindingFlags).Select(x => x.Name.ToLowerInvariant()).ToList();
            names.AddRange(entityType.GetProperties(bindingFlags).Select(x => x.Name.ToLowerInvariant()));

            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var hasExpectedConstructor = constructors.Any(constructorInfo =>
            {
                var parameters = constructorInfo.GetParameters().Select(x => x.Name?.ToLowerInvariant()).ToList();
                return names.Intersect(parameters).Count() == names.Count;
            });

            if (!hasExpectedConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void DomainEventShouldHaveDomainEventPostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEventBase))
            .Or()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void BusinessRuleShouldHaveRulePostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IBusinessRule))
            .Should()
            .HaveNameEndingWith("Rule")
            .GetResult();

        AssertArchTestResult(result);
    }
}
