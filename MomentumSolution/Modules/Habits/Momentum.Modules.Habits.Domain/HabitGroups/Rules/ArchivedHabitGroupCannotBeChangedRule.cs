using Momentum.BuildingBlocks.Domain;

namespace Momentum.Modules.Habits.Domain.HabitGroups.Rules;

internal class ArchivedHabitGroupCannotBeChangedRule : IBusinessRule
{
    private readonly bool _isArchived;

    internal ArchivedHabitGroupCannotBeChangedRule(bool isArchived)
    {
        _isArchived = isArchived;
    }

    public bool IsBroken() => _isArchived;

    public string Message => "Archived habit group cannot be changed.";
}
