using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.Configuration.Commands;

public abstract class InternalCommandBase : CommandBase
{
    protected InternalCommandBase(Guid id)
        : base(id)
    {
    }
}
