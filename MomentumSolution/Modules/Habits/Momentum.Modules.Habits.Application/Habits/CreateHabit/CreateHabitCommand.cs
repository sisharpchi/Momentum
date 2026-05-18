using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.Habits.CreateHabit;

public class CreateHabitCommand : CommandBase<Guid>
{
    public CreateHabitCommand(
        Guid userId,
        Guid groupId,
        string title,
        string? description,
        string iconType,
        string iconValue,
        string color,
        string habitType,
        string scheduleFrequency,
        DateOnly startDate,
        DateOnly? endDate,
        string timezone,
        IReadOnlyCollection<DayOfWeek> daysOfWeek,
        decimal? targetValue,
        string? targetUnit,
        int sortOrder)
    {
        UserId = userId;
        GroupId = groupId;
        Title = title;
        Description = description;
        IconType = iconType;
        IconValue = iconValue;
        Color = color;
        HabitType = habitType;
        ScheduleFrequency = scheduleFrequency;
        StartDate = startDate;
        EndDate = endDate;
        Timezone = timezone;
        DaysOfWeek = daysOfWeek;
        TargetValue = targetValue;
        TargetUnit = targetUnit;
        SortOrder = sortOrder;
    }

    public Guid UserId { get; }

    public Guid GroupId { get; }

    public string Title { get; }

    public string? Description { get; }

    public string IconType { get; }

    public string IconValue { get; }

    public string Color { get; }

    public string HabitType { get; }

    public string ScheduleFrequency { get; }

    public DateOnly StartDate { get; }

    public DateOnly? EndDate { get; }

    public string Timezone { get; }

    public IReadOnlyCollection<DayOfWeek> DaysOfWeek { get; }

    public decimal? TargetValue { get; }

    public string? TargetUnit { get; }

    public int SortOrder { get; }
}
