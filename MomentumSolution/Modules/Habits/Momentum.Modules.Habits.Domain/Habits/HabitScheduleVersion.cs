using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits;

public class HabitScheduleVersion : ValueObject
{
    public HabitSchedule Schedule { get; }

    public DateOnly EffectiveFrom { get; }

    public DateOnly? EffectiveTo { get; private set; }

    private HabitScheduleVersion()
    {
        Schedule = null!;
    }

    private HabitScheduleVersion(HabitSchedule schedule, DateOnly effectiveFrom, DateOnly? effectiveTo)
    {
        Schedule = schedule;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
    }

    public static HabitScheduleVersion Start(HabitSchedule schedule) =>
        new(schedule, schedule.StartDate, null);

    internal void CloseOn(DateOnly effectiveTo)
    {
        EffectiveTo = effectiveTo;
    }
}
