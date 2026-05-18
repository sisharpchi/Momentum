using Momentum.BuildingBlocks.Infrastructure.EventBus;

namespace Momentum.Modules.Habits.IntegrationEvents;

public class HabitCreatedIntegrationEvent : IntegrationEvent
{
    public HabitCreatedIntegrationEvent(Guid habitId, Guid userId, string title)
        : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        HabitId = habitId;
        UserId = userId;
        Title = title;
    }

    public Guid HabitId { get; }

    public Guid UserId { get; }

    public string Title { get; }
}
