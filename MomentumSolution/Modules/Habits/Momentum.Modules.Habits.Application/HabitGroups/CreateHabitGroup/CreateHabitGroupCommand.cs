using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.HabitGroups.CreateHabitGroup;

public class CreateHabitGroupCommand : CommandBase<Guid>
{
    public CreateHabitGroupCommand(
        Guid userId,
        string name,
        string iconType,
        string iconValue,
        string color,
        int sortOrder)
    {
        UserId = userId;
        Name = name;
        IconType = iconType;
        IconValue = iconValue;
        Color = color;
        SortOrder = sortOrder;
    }

    public Guid UserId { get; }

    public string Name { get; }

    public string IconType { get; }

    public string IconValue { get; }

    public string Color { get; }

    public int SortOrder { get; }
}
