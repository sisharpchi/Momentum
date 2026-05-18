using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.HabitGroups;

public class HabitGroupId : TypedIdValueBase
{
    public HabitGroupId(Guid value)
        : base(value)
    {
    }
}
