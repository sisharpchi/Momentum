using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.Habits.Rules;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitSchedule : ValueObject
{
    public HabitScheduleFrequency Frequency { get; }

    public DateOnly StartDate { get; }

    public DateOnly? EndDate { get; }

    public string Timezone { get; }

    public IReadOnlyCollection<DayOfWeek> DaysOfWeek => _daysOfWeek.AsReadOnly();

    private readonly List<DayOfWeek> _daysOfWeek;

    private HabitSchedule()
    {
        Frequency = null!;
        Timezone = string.Empty;
        _daysOfWeek = [];
    }

    private HabitSchedule(
        HabitScheduleFrequency frequency,
        DateOnly startDate,
        string timezone,
        IEnumerable<DayOfWeek> daysOfWeek,
        DateOnly? endDate)
    {
        CheckRule(new HabitTimezoneCannotBeEmptyRule(timezone));
        CheckRule(new HabitScheduleEndDateCannotBeBeforeStartDateRule(startDate, endDate));

        var normalizedDays = daysOfWeek.Distinct().OrderBy(day => day).ToList();
        if (frequency == HabitScheduleFrequency.CustomWeekly)
        {
            CheckRule(new CustomWeeklyScheduleMustContainDaysRule(normalizedDays));
        }

        Frequency = frequency;
        StartDate = startDate;
        EndDate = endDate;
        Timezone = timezone.Trim();
        _daysOfWeek = normalizedDays;
    }

    public static HabitSchedule Daily(DateOnly startDate, string timezone, DateOnly? endDate = null) =>
        new(HabitScheduleFrequency.Daily, startDate, timezone, [], endDate);

    public static HabitSchedule Weekly(DateOnly startDate, string timezone, DateOnly? endDate = null) =>
        new(HabitScheduleFrequency.Weekly, startDate, timezone, [], endDate);

    public static HabitSchedule CustomWeekly(
        DateOnly startDate,
        string timezone,
        IEnumerable<DayOfWeek> daysOfWeek,
        DateOnly? endDate = null) =>
        new(HabitScheduleFrequency.CustomWeekly, startDate, timezone, daysOfWeek, endDate);
}
