using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class CustomWeeklyScheduleMustContainDaysRule : IBusinessRule
{
    private readonly IReadOnlyCollection<DayOfWeek> _daysOfWeek;

    internal CustomWeeklyScheduleMustContainDaysRule(IReadOnlyCollection<DayOfWeek> daysOfWeek)
    {
        _daysOfWeek = daysOfWeek;
    }

    public bool IsBroken() => _daysOfWeek.Count == 0;

    public string Message => "Custom weekly habit schedule must contain at least one day.";
}
