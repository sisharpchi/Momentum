using Momentum.Modules.Habits.Application.HabitGroups.GetHabitGroups;
using Momentum.Modules.Habits.IntegrationTests.SeedWork;

namespace Momentum.Modules.Habits.IntegrationTests.HabitGroups;

public class CreateHabitGroupTests : TestBase
{
    [Fact]
    public async Task CreateHabitGroupCommandPersistsGroupReadableByQuery()
    {
        await using var context = await CreateContextAsync();
        var userId = Guid.NewGuid();

        var groupId = await CreateGroupAsync(context, userId, "Work", 2);
        await CommitAsync(context);
        context.ChangeTracker.Clear();

        var groups = await new GetHabitGroupsQueryHandler(context).Handle(
            new GetHabitGroupsQuery(userId),
            CancellationToken.None);

        Assert.Single(groups);
        Assert.Equal(groupId, groups[0].Id);
        Assert.Equal("Work", groups[0].Name);
        Assert.Equal("Emoji", groups[0].IconType);
        Assert.Equal("#3B82F6", groups[0].Color);
    }
}
