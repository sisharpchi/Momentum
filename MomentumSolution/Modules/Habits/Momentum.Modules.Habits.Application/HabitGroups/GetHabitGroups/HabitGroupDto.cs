namespace Momentum.Modules.Habits.Application.HabitGroups.GetHabitGroups;

public class HabitGroupDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string IconType { get; set; } = string.Empty;

    public string IconValue { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsArchived { get; set; }
}
