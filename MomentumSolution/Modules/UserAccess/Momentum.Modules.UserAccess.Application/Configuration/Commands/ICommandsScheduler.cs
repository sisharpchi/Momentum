using Momentum.Modules.UserAccess.Application.Contracts;

namespace Momentum.Modules.UserAccess.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);

    Task EnqueueAsync<T>(ICommand<T> command);
}
