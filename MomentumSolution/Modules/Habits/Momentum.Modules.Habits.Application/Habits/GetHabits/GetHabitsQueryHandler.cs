using Microsoft.EntityFrameworkCore;
using Momentum.Modules.Habits.Application.Configuration.Queries;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Application.Habits.GetHabits;

public class GetHabitsQueryHandler : IQueryHandler<GetHabitsQuery, IReadOnlyList<HabitDto>>
{
    private readonly DbContext _context;

    public GetHabitsQueryHandler(DbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<HabitDto>> Handle(GetHabitsQuery query, CancellationToken cancellationToken)
    {
        var habits = await _context.Set<Habit>()
            .Where(habit => habit.UserId.Value == query.UserId)
            .ToListAsync(cancellationToken);

        return habits
            .Where(habit => habit.Status != HabitStatus.Archived)
            .OrderBy(habit => habit.SortOrder)
            .Select(habit => new HabitDto
        {
            Id = habit.Id.Value,
            UserId = habit.UserId.Value,
            GroupId = habit.GroupId.Value,
            Title = habit.Title,
            Description = habit.Description,
            IconType = habit.Icon.Type,
            IconValue = habit.Icon.Value,
            Color = habit.Color.Value,
            Type = habit.Type.Value,
            Status = habit.Status.Value,
            Frequency = habit.CurrentSchedule.Frequency.Value,
            TargetValue = habit.Target?.Value,
            TargetUnit = habit.Target?.Unit,
            SortOrder = habit.SortOrder,
        }).ToList();
    }
}
