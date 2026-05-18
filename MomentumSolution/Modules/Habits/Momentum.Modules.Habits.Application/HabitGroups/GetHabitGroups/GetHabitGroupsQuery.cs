using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.HabitGroups.GetHabitGroups;

public class GetHabitGroupsQuery : QueryBase<IReadOnlyList<HabitGroupDto>>
{
    public GetHabitGroupsQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
