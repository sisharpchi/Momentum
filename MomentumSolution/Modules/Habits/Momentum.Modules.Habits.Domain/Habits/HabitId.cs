using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitId : TypedIdValueBase
{
    public HabitId(Guid value)
        : base(value)
    {
    }
}
