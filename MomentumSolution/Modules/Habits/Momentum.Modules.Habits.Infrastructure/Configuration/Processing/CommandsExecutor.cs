using Autofac;
using MediatR;
using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Infrastructure.Configuration.Processing;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using var scope = HabitsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(command);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using var scope = HabitsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();

        return await mediator.Send(command);
    }
}
