using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Habits.Application.Configuration.Commands;
using Momentum.Modules.Habits.Domain.HabitGroups;
using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Application.Habits.CreateHabit;

public class CreateHabitCommandHandler : ICommandHandler<CreateHabitCommand, Guid>
{
    private readonly IMainRepository _repository;

    public CreateHabitCommandHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateHabitCommand command, CancellationToken cancellationToken)
    {
        var habit = Habit.Create(
            new UserId(command.UserId),
            new HabitGroupId(command.GroupId),
            command.Title,
            command.Description,
            HabitIconFactory.Create(command.IconType, command.IconValue),
            HabitColor.FromHex(command.Color),
            ParseHabitType(command.HabitType),
            ParseSchedule(command),
            ParseTarget(command),
            command.SortOrder);

        await _repository.Set<Habit>().AddAsync(habit, cancellationToken);

        return habit.Id.Value;
    }

    private static HabitType ParseHabitType(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "good" => HabitType.Good,
            "bad" => HabitType.Bad,
            "time" => HabitType.Time,
            "count" => HabitType.Count,
            "health" => HabitType.Health,
            _ => HabitType.Good,
        };
    }

    private static HabitSchedule ParseSchedule(CreateHabitCommand command)
    {
        return command.ScheduleFrequency.Trim().ToLowerInvariant() switch
        {
            "weekly" => HabitSchedule.Weekly(command.StartDate, command.Timezone, command.EndDate),
            "customweekly" or "custom" => HabitSchedule.CustomWeekly(
                command.StartDate,
                command.Timezone,
                command.DaysOfWeek,
                command.EndDate),
            _ => HabitSchedule.Daily(command.StartDate, command.Timezone, command.EndDate),
        };
    }

    private static HabitTarget? ParseTarget(CreateHabitCommand command)
    {
        if (!command.TargetValue.HasValue || string.IsNullOrWhiteSpace(command.TargetUnit))
        {
            return null;
        }

        return HabitTarget.Count(command.TargetValue.Value, command.TargetUnit);
    }
}
