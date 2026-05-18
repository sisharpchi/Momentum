using Momentum.Modules.Habits.Application.Habits.GetHabits;
using Momentum.Modules.Habits.IntegrationTests.SeedWork;

namespace Momentum.Modules.Habits.IntegrationTests.Habits;

public class CreateHabitTests : TestBase
{
    [Fact]
    public async Task CreateHabitCommandPersistsHabitReadableByQuery()
    {
        await using var context = await CreateContextAsync();
        var userId = Guid.NewGuid();

        var groupId = await CreateGroupAsync(context, userId);
        var habitId = await CreateHabitAsync(context, userId, groupId);

        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var habits = await new GetHabitsQueryHandler(context).Handle(new GetHabitsQuery(userId), CancellationToken.None);

        Assert.Single(habits);
        Assert.Equal(habitId, habits[0].Id);
        Assert.Equal("Drink 2L water", habits[0].Title);
        Assert.Equal("Daily", habits[0].Frequency);
        Assert.Equal("Active", habits[0].Status);
    }

    [Fact]
    public async Task GetHabitsQueryExcludesArchivedHabitsAndSortsBySortOrder()
    {
        await using var context = await CreateContextAsync();
        var userId = Guid.NewGuid();
        var groupId = await CreateGroupAsync(context, userId);
        await CreateHabitAsync(context, userId, groupId, "Second", 2);
        var firstHabitId = await CreateHabitAsync(context, userId, groupId, "First", 1);
        var archivedHabitId = await CreateHabitAsync(context, userId, groupId, "Archived", 0);
        await CommitAsync(context);

        var archivedHabit = await context.Habits.FindAsync(new Momentum.Modules.Habits.Domain.Habits.HabitId(archivedHabitId));
        archivedHabit!.Archive();
        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var habits = await new GetHabitsQueryHandler(context).Handle(new GetHabitsQuery(userId), CancellationToken.None);

        Assert.Equal(2, habits.Count);
        Assert.Equal(firstHabitId, habits[0].Id);
        Assert.DoesNotContain(habits, habit => habit.Id == archivedHabitId);
    }
}
