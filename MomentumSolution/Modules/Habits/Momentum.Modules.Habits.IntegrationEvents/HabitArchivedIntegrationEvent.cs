using Momentum.BuildingBlocks.Infrastructure.EventBus;

namespace Momentum.Modules.Habits.IntegrationEvents;

public class HabitArchivedIntegrationEvent : IntegrationEvent
{
    public HabitArchivedIntegrationEvent(Guid habitId, Guid userId)
        : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        HabitId = habitId;
        UserId = userId;
    }

    public Guid HabitId { get; }

    public Guid UserId { get; }
}
