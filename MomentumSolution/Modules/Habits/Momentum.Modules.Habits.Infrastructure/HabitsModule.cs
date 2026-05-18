using Autofac;
using MediatR;
using Momentum.Modules.Habits.Application.Contracts;
using Momentum.Modules.Habits.Infrastructure.Configuration;
using Momentum.Modules.Habits.Infrastructure.Configuration.Processing;

namespace Momentum.Modules.Habits.Infrastructure;

public class HabitsModule : IHabitsModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = HabitsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();

        return await mediator.Send(query);
    }
}
