using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);
}
