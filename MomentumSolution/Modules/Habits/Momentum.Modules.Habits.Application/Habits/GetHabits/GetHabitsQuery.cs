using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.Habits.GetHabits;

public class GetHabitsQuery : QueryBase<IReadOnlyList<HabitDto>>
{
    public GetHabitsQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}
