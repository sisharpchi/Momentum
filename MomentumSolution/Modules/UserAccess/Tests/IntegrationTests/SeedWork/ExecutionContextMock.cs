using Momentum.BuildingBlocks.Application;

namespace Momentum.Modules.UserAccess.IntegrationTests.SeedWork;

public class ExecutionContextMock : IExecutionContextAccessor
{
    public ExecutionContextMock(Guid userId)
    {
        UserId = userId;
        CorrelationId = Guid.NewGuid();
        IsAvailable = true;
    }

    public Guid UserId { get; private set; }

    public Guid CorrelationId { get; }

    public bool IsAvailable { get; }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }
}
