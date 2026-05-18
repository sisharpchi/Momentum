namespace Momentum.Modules.Habits.Application.Habits.GetHabits;

public class HabitDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string IconType { get; set; } = string.Empty;

    public string IconValue { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Frequency { get; set; } = string.Empty;

    public decimal? TargetValue { get; set; }

    public string? TargetUnit { get; set; }

    public int SortOrder { get; set; }
}
