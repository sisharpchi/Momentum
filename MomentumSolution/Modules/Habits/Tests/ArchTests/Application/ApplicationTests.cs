using System.Reflection;
using FluentValidation;
using MediatR;
using Momentum.Modules.Habits.Application.Configuration.Commands;
using Momentum.Modules.Habits.Application.Configuration.Queries;
using Momentum.Modules.Habits.Application.Contracts;
using Momentum.Modules.Habits.ArchTests.SeedWork;
using NetArchTest.Rules;
using Newtonsoft.Json;

namespace Momentum.Modules.Habits.ArchTests.Application;

public class ApplicationTests : TestBase
{
    [Fact]
    public void CommandShouldBeImmutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(CommandBase))
            .Or()
            .Inherit(typeof(CommandBase<>))
            .Or()
            .Inherit(typeof(InternalCommandBase))
            .Or()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Fact]
    public void QueryShouldBeImmutable()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .GetTypes();

        AssertAreImmutable(types);
    }

    [Fact]
    public void CommandHandlerShouldHaveNameEndingWithCommandHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .And()
            .DoNotHaveNameMatching(".*Decorator.*")
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void QueryHandlerShouldHaveNameEndingWithQueryHandler()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void CommandAndQueryHandlersShouldNotBePublic()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult()
            .FailingTypes;

        AssertFailingTypes(types);
    }

    [Fact]
    public void ValidatorShouldHaveNameEndingWithValidator()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        AssertArchTestResult(result);
    }

    [Fact]
    public void ValidatorsShouldNotBePublic()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .NotBePublic()
            .GetResult()
            .FailingTypes;

        AssertFailingTypes(types);
    }

    [Fact]
    public void InternalCommandShouldHaveJsonConstructorAttribute()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(InternalCommandBase))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var type in types)
        {
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var hasJsonConstructor = constructors.Any(constructor =>
                constructor.GetCustomAttributes(typeof(JsonConstructorAttribute), false).Length > 0);

            if (!hasJsonConstructor)
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void MediatRRequestHandlerShouldNotBeUsedDirectly()
    {
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .DoNotHaveName("ICommandHandler`1")
            .Should()
            .ImplementInterface(typeof(IRequestHandler<>))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var type in types)
        {
            var isCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            var isCommandWithResultHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            var isQueryHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            if (!isCommandHandler && !isCommandWithResultHandler && !isQueryHandler)
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void CommandWithResultShouldNotReturnUnit()
    {
        var commandWithResultHandlerType = typeof(ICommandHandler<,>);
        var types = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(commandWithResultHandlerType)
            .GetTypes()
            .ToList();

        List<Type> failingTypes = [];
        foreach (var type in types)
        {
            var interfaceType = type.GetInterface(commandWithResultHandlerType.Name);
            if (interfaceType?.GenericTypeArguments[1] == typeof(Unit))
            {
                failingTypes.Add(type);
            }
        }

        AssertFailingTypes(failingTypes);
    }
}
