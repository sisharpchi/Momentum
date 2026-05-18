using Momentum.BuildingBlocks.Infrastructure.EventBus;
using Momentum.Modules.Habits.IntegrationEvents;

namespace Momentum.Modules.Habits.IntegrationTests.IntegrationEvents;

public class HabitCreatedIntegrationEventTests
{
    [Fact]
    public void HabitCreatedIntegrationEventCarriesHabitIdentity()
    {
        var habitId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var @event = new HabitCreatedIntegrationEvent(habitId, userId, "Drink 2L water");

        Assert.IsAssignableFrom<IntegrationEvent>(@event);
        Assert.Equal(habitId, @event.HabitId);
        Assert.Equal(userId, @event.UserId);
        Assert.Equal("Drink 2L water", @event.Title);
    }

    [Fact]
    public void HabitArchivedIntegrationEventCarriesHabitIdentity()
    {
        var habitId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var @event = new HabitArchivedIntegrationEvent(habitId, userId);

        Assert.IsAssignableFrom<IntegrationEvent>(@event);
        Assert.Equal(habitId, @event.HabitId);
        Assert.Equal(userId, @event.UserId);
    }
}
