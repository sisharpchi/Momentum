using System.Reflection;
using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Domain.Users;
using Momentum.Modules.UserAccess.Infrastructure;
using NetArchTest.Rules;

namespace Momentum.Modules.UserAccess.ArchTests.SeedWork;

public abstract class TestBase
{
    protected static Assembly ApplicationAssembly => typeof(CommandBase).Assembly;

    protected static Assembly DomainAssembly => typeof(User).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(UserAccessContext).Assembly;

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
