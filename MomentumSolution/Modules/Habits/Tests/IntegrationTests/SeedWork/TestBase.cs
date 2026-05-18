using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure;
using Momentum.Modules.Habits.Application.HabitGroups.CreateHabitGroup;
using Momentum.Modules.Habits.Application.Habits.CreateHabit;
using Momentum.Modules.Habits.Infrastructure;
using Momentum.Modules.Habits.Infrastructure.Domain.Habits;

namespace Momentum.Modules.Habits.IntegrationTests.SeedWork;

public abstract class TestBase
{
    protected static async Task<HabitsContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<HabitsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new HabitsContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    protected static HabitRepository CreateRepository(HabitsContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        return new HabitRepository(context, unitOfWork);
    }

    protected static Task CommitAsync(HabitsContext context)
    {
        var unitOfWork = new UnitOfWork(context, new NoOpDomainEventsDispatcher());
        return unitOfWork.CommitAsync(CancellationToken.None);
    }

    protected static async Task<Guid> CreateGroupAsync(
        HabitsContext context,
        Guid userId,
        string name = "Health",
        int sortOrder = 1)
    {
        var repository = CreateRepository(context);
        return await new CreateHabitGroupCommandHandler(repository).Handle(
            new CreateHabitGroupCommand(userId, name, "emoji", "heart", "#3B82F6", sortOrder),
            CancellationToken.None);
    }

    protected static async Task<Guid> CreateHabitAsync(
        HabitsContext context,
        Guid userId,
        Guid groupId,
        string title = "Drink 2L water",
        int sortOrder = 1)
    {
        var repository = CreateRepository(context);
        return await new CreateHabitCommandHandler(repository).Handle(
            new CreateHabitCommand(
                userId,
                groupId,
                title,
                "Stay hydrated",
                "emoji",
                "water",
                "#3B82F6",
                "Count",
                "Daily",
                new DateOnly(2026, 5, 18),
                null,
                "Asia/Tashkent",
                [],
                2000,
                "ml",
                sortOrder),
            CancellationToken.None);
    }
}
