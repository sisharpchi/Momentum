using Momentum.BuildingBlocks.Application.Outbox;

namespace Momentum.Modules.Habits.Infrastructure.Outbox;

public class OutboxAccessor
{
    private readonly HabitsContext _context;

    public OutboxAccessor(HabitsContext context)
    {
        _context = context;
    }

    public void Add(OutboxMessage message)
    {
        _context.OutboxMessages.Add(message);
    }

    public Task Save()
    {
        return Task.CompletedTask;
    }
}
