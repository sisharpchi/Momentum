using System.Reflection;
using Momentum.Modules.Registrations.Application.Contracts;
using Momentum.Modules.Registrations.Domain.UserRegistrations;
using Momentum.Modules.Registrations.Infrastructure;
using NetArchTest.Rules;

namespace Momentum.Modules.Registrations.ArchTests.SeedWork;

public abstract class TestBase
{
    protected static Assembly ApplicationAssembly => typeof(CommandBase).Assembly;

    protected static Assembly DomainAssembly => typeof(UserRegistration).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(RegistrationsContext).Assembly;

    protected static void AssertAreImmutable(IEnumerable<Type> types)
    {
        List<Type> failingTypes = [];
        foreach (var type in types)
        {
            if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
            {
                failingTypes.Add(type);
                break;
            }
        }

        AssertFailingTypes(failingTypes);
    }

    protected static void AssertFailingTypes(IEnumerable<Type>? types)
    {
        Assert.Empty(types ?? []);
    }

    protected static void AssertArchTestResult(TestResult result)
    {
        AssertFailingTypes(result.FailingTypes);
    }
}
