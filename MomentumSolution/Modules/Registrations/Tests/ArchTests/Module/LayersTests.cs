using Momentum.Modules.Registrations.ArchTests.SeedWork;
using NetArchTest.Rules;

namespace Momentum.Modules.Registrations.ArchTests.Module;

public class LayersTests : TestBase
{
    [Fact]
    public void DomainLayerDoesNotHaveDependencyToApplicationLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void DomainLayerDoesNotHaveDependencyToInfrastructureLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void ApplicationLayerDoesNotHaveDependencyToInfrastructureLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        AssertArchTestResult(result);
    }
}
