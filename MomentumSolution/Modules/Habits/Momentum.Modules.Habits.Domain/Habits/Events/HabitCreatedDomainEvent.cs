using Momentum.BuildingBlocks.Domain;
using Momentum.Modules.Habits.Domain.HabitGroups;

namespace Momentum.Modules.Habits.Domain.Habits.Events;

public class HabitCreatedDomainEvent : DomainEventBase
{
    public HabitCreatedDomainEvent(HabitId habitId, UserId userId, HabitGroupId groupId)
    {
        HabitId = habitId;
        UserId = userId;
        GroupId = groupId;
    }

    public HabitId HabitId { get; }

    public UserId UserId { get; }

    public HabitGroupId GroupId { get; }
}
