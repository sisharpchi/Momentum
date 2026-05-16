using Momentum.Modules.Registrations.Application.Contracts;

namespace Momentum.Modules.Registrations.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}