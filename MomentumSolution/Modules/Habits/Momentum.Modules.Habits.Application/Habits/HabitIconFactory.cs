using Momentum.Modules.Habits.Domain.Habits;

namespace Momentum.Modules.Habits.Application.Habits;

internal static class HabitIconFactory
{
    internal static HabitIcon Create(string type, string value)
    {
        return type.Trim().ToLowerInvariant() switch
        {
            "emoji" => HabitIcon.Emoji(value),
            "text" => HabitIcon.Text(value),
            "image" => HabitIcon.Image(value),
            _ => HabitIcon.Text(value),
        };
    }
}
