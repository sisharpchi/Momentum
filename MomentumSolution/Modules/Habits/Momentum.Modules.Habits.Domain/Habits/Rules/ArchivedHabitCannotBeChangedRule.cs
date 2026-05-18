using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.Habits.Rules;

internal class ArchivedHabitCannotBeChangedRule : IBusinessRule
{
    private readonly HabitStatus _status;

    internal ArchivedHabitCannotBeChangedRule(HabitStatus status)
    {
        _status = status;
    }

    public bool IsBroken() => _status == HabitStatus.Archived;

    public string Message => "Archived habit cannot be changed.";
}
