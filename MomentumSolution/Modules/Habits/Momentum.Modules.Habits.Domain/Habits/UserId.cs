using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class UserId : TypedIdValueBase
{
    public UserId(Guid value)
        : base(value)
    {
    }
}
