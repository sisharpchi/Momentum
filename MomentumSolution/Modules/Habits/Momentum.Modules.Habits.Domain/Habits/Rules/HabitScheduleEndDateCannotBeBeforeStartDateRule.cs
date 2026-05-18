using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class HabitScheduleEndDateCannotBeBeforeStartDateRule : IBusinessRule
{
    private readonly DateOnly _startDate;
    private readonly DateOnly? _endDate;

    internal HabitScheduleEndDateCannotBeBeforeStartDateRule(DateOnly startDate, DateOnly? endDate)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    public bool IsBroken() => _endDate.HasValue && _endDate.Value < _startDate;

    public string Message => "Habit schedule end date cannot be before start date.";
}
